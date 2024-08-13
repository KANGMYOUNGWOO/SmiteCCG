
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



public class CharacterManager : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
    private ResourceManager resourceManager;
    private UIManager uiManager;

    #region PlayerData
    //손패 카드 정보
    public CardData[] playerCards = new CardData[10];
    public List<GodBase> Gods  = new List<GodBase>();
    //
    public int[,] FactionArray = new int[7, 5];
    
    public int playerPower { get; set; } = 0;
    public int playerMana { get; set; } = 0;
    public int playerMaxMana { get; set; } = 0;

    public string ID { get; set; }
    private int DeckDrawIndex = 0;
    #endregion
    public CardData[] playerHand = new CardData[6];

    public int CurrentPosition = 0;
    public UnityEvent onManaChange;

    //public 

    
  
    public GameScene gamescene { get; set; }
    private CardData CurrentCard = null;

   public int currentHeroCardIndex { get; set; } = -1;
    private int currentSpellCardIndex = -1;


    private void Awake()
    {
        resourceManager = GameManager.GetManagerClass<ResourceManager>();
        uiManager = GameManager.GetManagerClass<UIManager>();
        Initialize();
    }

    private void Initialize()
    {
     
        CardData cardData = new CardData();
        cardData.CardName = "";
        cardData.CardCost = 0;
        cardData.CardBarrier = 0;
        cardData.CardPower = 0;
        cardData.CardId = 0;
        cardData.cardType = CardData.CardType.Hero;
        Gods.Clear();
        for (int i = 0; i < playerCards.Length; i++) playerCards[i] = cardData;
        for (int j = 0; j < playerHand.Length; j++) playerHand[j] = cardData;
        for(int l=0;l<5;l++)
        {
            for(int c=0;c<7;c++)
            {                               
                FactionArray[c,l] = 0;
            }
        }
       
    }


    //나의 덱이 완성되었는지 return
    public bool CheckDeckFull()
    {
        bool bisFull = true;
        
        for (int i = 0; i < playerCards.Length; i++)
        {
            if (playerCards[i].CardId == 0)
            {               
                bisFull = false;
                break;
            }
        }
       
        return bisFull;
    }
   
    //덱이 완전히 
    public bool CheckCardListEmpty()
    {
        return playerCards[0].CardId == 0;
    }

    public void TurnStart()
    {
        playerMaxMana += 2;
        if (playerMana >= 10) playerMana = 10;
        playerMana = playerMaxMana;
        onManaChange.Invoke();
        gamescene.TurnStart();
    }

    private void DrawCard()
    {
        playerHand[DeckDrawIndex] = playerCards[DeckDrawIndex];       
        gamescene.DrawCard(DeckDrawIndex);
        DeckDrawIndex++;
    }


    private void Shuffle()
    {
        int random1, random2;
        CardData temp = null;
        
        for (int i = 0; i < playerHand.Length; ++i)
        {
            random1 = UnityEngine.Random.Range(0, playerHand.Length);
            random2 = UnityEngine.Random.Range(0, playerHand.Length);

            temp = playerCards[random1];
            playerCards[random1] = playerCards[random2];
            playerCards[random2] = temp;
        }
    }



    public void SetCurrentCard(int index)
    {
        CurrentCard = playerHand[index]; 
    }
    
    public void ShowGUI(int slotIndex)
    {
        
        int target_x = slotIndex / 10;
        int target_y = slotIndex % 10;
   

        switch(CurrentCard.cardType)
        {
            case CardData.CardType.Hero:
                if (currentHeroCardIndex == slotIndex) return;
                currentHeroCardIndex = slotIndex;
                gamescene.ChangeSlotFaction(slotIndex);
                break;

            case CardData.CardType.Spell:
                //if (currentHeroCardIndex == slotIndex) return;
                if (currentSpellCardIndex == slotIndex) return;
                currentSpellCardIndex = slotIndex;
                if (FactionArray[target_x, target_y] != 1)
                {
                    gamescene.ChangeSlotColor(slotIndex);
                    
                    for (int i = 0; i < Gods.Count; i++)
                    {
                        if (Gods[i].Positon / 10 != target_x && Gods[i].Positon % 10 != target_y) Gods[i].Idle();
                        //Gods[i].Idle();
                    }
                }
                else
                {
                    gamescene.ChangeSlotColor(slotIndex, ref CurrentCard.DamageArea);
                    for (int i = 0; i < Gods.Count; i++)
                    {
                      
                        if (Gods[i].Positon / 10 == target_x && Gods[i].Positon % 10 == target_y)
                        {
                            Gods[i].Ready();
                        }
                    }
                }
                    //  gamescene.ChangeSlotColor(CurrentCard,index);
                break;
        }
    }

    public bool CheckGameCard(int index)
    {
        int target_x = index / 10;
        int target_y = index % 10;
        bool bisOk = false;

        switch (CurrentCard.cardType)
        {
            case CardData.CardType.Hero:
                bisOk = FactionArray[target_x, target_y] == 0;
                if (!bisOk) Debug.Log(string.Format("현재 내려는 곳에 이미 무언가 있습니다. 값->{0}", FactionArray[target_x, target_y]));
                break;

            case CardData.CardType.Spell:
                bisOk = FactionArray[target_x, target_y] == 1;
                break;
        }

        return bisOk;
    }

    public void SetGameCard(int index)
    {
        int target_x = index / 10;
        int target_y = index % 10;

        WaitForSeconds wait = new WaitForSeconds(0.5f);
        IEnumerator SpawnWait()
        {
            yield return wait;
            resourceManager.SpawnModel(string.Format("Gods/{0}", CurrentCard.CardId), gamescene.GetSlotPosition(index));
        }


        switch (CurrentCard.cardType)
        {
            case CardData.CardType.Hero:
                //GodBase god = null;
                FactionArray[target_x, target_y] = 1;
               
                gamescene.ChangeSlotFaction(index, 1);
                StartCoroutine(SpawnWait());
                currentHeroCardIndex = index;
                break;

            case CardData.CardType.Spell:

                break;
        }

        playerMana -= CurrentCard.CardCost;
        onManaChange.Invoke();

        for(int i= index; i<playerHand.Length-1;i++)
        {
            //CardData cd = playerHand[i];
            if (playerHand[i].CardId == 0) break;
            playerHand[i] = playerHand[i + 1];
            gamescene.DrawCard(i);
            
        }
    }

    public void SpawnGod(GameObject gobject)
    {
        GodBase god = gobject.GetComponent<GodBase>();
        Debug.Log("success");
        god.SetPower(CurrentCard.CardPower, CurrentCard.CardBarrier);
        god.Spawn();
        Gods.Add(god);
        Debug.Log(Gods.Count);
        god.transform.rotation = Quaternion.Euler(0, 90, 0);
    }


    public bool CheckMana()
    {
        if(CurrentCard.CardCost > playerMana)
        Debug.Log(string.Format("현재 마나 : {0} , 사용하려는 카드 마나 : {1}",playerMana,CurrentCard.CardCost));
        return  CurrentCard.CardCost <= playerMana;            
    }

    public void AddPlayerCard(int index) 
    { 
        if (CheckDeckFull()) return;
        for(int i=0;i<playerCards.Length;i++)
        {
            if (playerCards[i].CardId == 0)
            { 
                playerCards[i] = resourceManager.GetCardData(uiManager.deckScene.DeckIndex * 12 + index);
                break; 
            }
        }

        uiManager.deckScene.AdjustInfo();
    }

    public void RemovePlayerCard(int index)
    {
        if(CheckCardListEmpty()) return;
        CardData cardData = new CardData();
        cardData.CardName = "";
        cardData.CardCost = 0;
        cardData.CardBarrier = 0;
        cardData.CardPower = 0;
        cardData.CardId = 0;
        cardData.cardType = CardData.CardType.Hero;
        for(int i = index; i<playerCards.Length-1; i++)
        {
            playerCards[i] = playerCards[i + 1];
        }
        playerCards[playerCards.Length - 1] = cardData;
        uiManager.deckScene.AdjustInfo();
    }


    public void GameStart()
    {
        playerMana = 0;
        playerMaxMana = 0;
        playerPower = 0;
        TurnStart();
        Shuffle();
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
        DrawCard();
    }


   
    
}
