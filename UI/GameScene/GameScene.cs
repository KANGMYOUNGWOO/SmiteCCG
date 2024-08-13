using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Linq;

public class GameScene : MonoBehaviour
{
    private CharacterManager characterManager;
    private ResourceManager resourceManager;
    private UIManager uIManager;

    private List<GameHeroCard> gameCards = new List<GameHeroCard>();
    private Canvas canvas;
    [SerializeField] private Transform slotParent;
    private Slot[,] slotArray = new Slot[7, 5];
    [SerializeField] private Transform cardTransform;
    [SerializeField] private TextMeshProUGUI TurnText;
    
    GameObject cursorLight = null;
    GameObject Spawn = null;
    GameObject Charge = null;

    #region

    [SerializeField] private TextMeshProUGUI myPowerText;
    [SerializeField] private TextMeshProUGUI enemyPowerText;
    [SerializeField] private TextMeshProUGUI myManaText;
    [SerializeField] private TextMeshProUGUI enemyManaText;
    [SerializeField] private vs versus;

    private HeartUI[,] hearts = new HeartUI[7, 5];
    private int colum = 0;
    private int row = 0;

    #endregion

    public void Initialize()
    {
        int row = 0;
        int col = 0;
        foreach (Transform tr in cardTransform)
        {
            GameHeroCard card = tr.GetComponent<GameHeroCard>();          
            gameCards.Add(card);
           
        }

        for (int i = 0; i < gameCards.Count; i++) { gameCards[i].Initialize(i,canvas,characterManager); gameCards[i].gameObject.SetActive(false); }

        foreach (Transform tr in slotParent)
        {
            Slot slot = tr.GetComponent<Slot>();
            slotArray[row, col] = slot;
            row++;
            if (row >= 7)
            {
                row = 0;
                col++;
            }
           
        }
      

        ResetSlot();
        EraseSlot();
       
        resourceManager.SpawnModel("cursorLight", Vector3.zero);
        resourceManager.SpawnModel("Spawn", Vector3.zero);
        resourceManager.SpawnModel("Charge", Vector3.zero);
       
        //cursorLight.SetActive(false);
    }

    public void GetcursorLight(GameObject game)
    {
        cursorLight = game;        
        //cursorLight.SetActive(false);
    }


    public void GetSpawn(GameObject game)
    {
        Spawn = game;
        Spawn.gameObject.SetActive(false);
    }


    public void GetCharge(GameObject game)
    {
        Charge = game;
        Charge.gameObject.SetActive(false);
    }
   

    private void Awake()
    {
        characterManager = GameManager.GetManagerClass<CharacterManager>();
        characterManager.gamescene = this;
        resourceManager = GameManager.GetManagerClass<ResourceManager>();
        uIManager = GameManager.GetManagerClass<UIManager>();
        uIManager.gameScene = this;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        //Initialize();

        
    }
    public void ChangeSlotFaction(int index)
    {
        int target_x = index / 10;
        int target_y = index % 10;
        
        if (target_x >6)Debug.Log(string.Format("( {0} , {1} )",target_y,target_y));
        if (target_y >4)Debug.Log(string.Format("( {0} , {1} )",target_y,target_y));
        if (target_x < 0) Debug.Log(string.Format("( {0} , {1} )", target_y, target_y));
        if (target_y < 0) Debug.Log(string.Format("( {0} , {1} )", target_y, target_y));

        ResetSlot();
        
        if (characterManager.FactionArray[target_x,target_y] == 0) slotArray[target_x, target_y].ChangeColor(2);
        else slotArray[target_x, target_y].ChangeColor(1);

        // Debug.Log(string.Format("{0} , {1}",target_x , target_y));

        cursorLight.SetActive(true);
        cursorLight.transform.position = slotArray[target_x, target_y].transform.position;       


    }

    public void ChangeSlotFaction(int index , int faction)
    {
        int target_x = index / 10;
        int target_y = index % 10;
        ResetSlot();
        Sprite color = null;
        WaitForSeconds wait = new WaitForSeconds(2.5f);
        IEnumerator SpawnEnd()
        {
            yield return wait;
            Spawn.gameObject.SetActive(false);
           
        }
        switch (faction)
        {
            case 0:
                slotArray[target_x, target_y].ChangeColor(resourceManager.GetSprite("gray"));                
                break;
            case 1:
                Spawn.gameObject.SetActive(true);
                Spawn.transform.position = slotArray[target_x, target_y].transform.position;
                StartCoroutine(SpawnEnd());
                slotArray[target_x, target_y].ChangeColor(resourceManager.GetSprite("blue"));
                break;
            case 2:
                Spawn.gameObject.SetActive(true);
                Spawn.transform.position = slotArray[target_x, target_y].transform.position;
                slotArray[target_x, target_y].ChangeColor(resourceManager.GetSprite("red"));
                break;
        
        }
        cursorLight.SetActive(false);
    }

    public void ChangeSlotColor(int index, ref int[] array)
    {
        int target_x = index / 10;
        int target_y = index % 10;

        int x = 0;
        int y = 0;

        ResetSlot();
        Charge.gameObject.SetActive(true);

        Charge.transform.position = new Vector3(slotArray[target_x, target_y].transform.position.x,1.31f, slotArray[target_x, target_y].transform.position.z);
        //slotArray[row, col].ChangeColor(1);        
        for (int i = 0; i < array.Length; i++)
        {
            x = (i % 3) - 1;
            y = 1 - (i / 3);
           //Debug.Log(string.Format("{0} , {1} : {2}",x,y , i));
            //slotArray[target_x + x, target_y + y].ChangeColor(array[i]);
            if (target_x + x >= 0 && target_x + x <= 6 && target_y + y >= 0 && target_y + y <= 4) slotArray[target_x + x, target_y + y].ChangeColor(array[i]);
            //slotArray[target_x + x, target_y + y].ChangeColor(array[i]);
        }
    }

    public void ChangeSlotColor(int index)
    {
        int target_x = index / 10;
        int target_y = index % 10;

        ResetSlot();
        Charge.gameObject.SetActive(false);
        slotArray[target_x, target_y].ChangeColor(1);
    }

    public void DrawCard(int index)
    {
        CardData card = characterManager.playerHand[index];
        gameCards[index].gameObject.SetActive(true);
        gameCards[index].SetCardInfo(card.sprite,card.CardCost,card.CardPower,card.CardBarrier,card.CardName,card.CardExplain);
    }


    public void ResetSlot()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                slotArray[i, j].ChangeColor(0);
            }
        }
    }

    public void EraseSlot()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                slotArray[i, j].ChangeColor(resourceManager.GetSprite("gray"));
            }
        }
    }


    public Vector3 GetSlotPosition(int index)
    {
        int target_x = index / 10;
        int target_y = index % 10;
        Vector3 slotpos = new Vector3(slotArray[target_x, target_y].transform.position.x, 0.3f, slotArray[target_x, target_y].transform.position.z);

        return slotpos;
    }
    
    public void StartGame(string text)
    {
        versus.startvs(text);
    }

    public void TurnStart()
    {
        TurnText.DOFade(0, 1.0f).From(1).SetEase(Ease.InOutBounce);
    }

    public void ReDrawUI()
    {
        myPowerText.text = string.Format("{0}", characterManager.playerPower);
        myManaText.text = string.Format("{0} / {1}", characterManager.playerMana, characterManager.playerMaxMana);
    }
}
