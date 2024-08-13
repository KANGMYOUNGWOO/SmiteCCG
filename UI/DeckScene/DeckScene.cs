using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;


public class DeckScene : MonoBehaviour
{
    private CharacterManager characterManager;
    private UIManager uiManager;
    private ResourceManager resourceManager;
    private Canvas canvas;

    //[SerializeField] private List<>
    [SerializeField] private Transform ListObject;
    [SerializeField] private Transform CardListObject;
    [SerializeField] private DeckDiscardUI deckDiscardUI;
    private List<DeckList> deckList = new List<DeckList>();
    
    [SerializeField]private List<DeckSettingHero> heroList = new List<DeckSettingHero>();
    [SerializeField]private List<DeckSettingSpell> spellList = new List<DeckSettingSpell>();

    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    public int DeckIndex { get; private set; } = 0;

    private void Awake()
    {       
        int decklistIndex = 0;
      
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
      
        characterManager = GameManager.GetManagerClass<CharacterManager>();
        uiManager = GameManager.GetManagerClass<UIManager>();
        resourceManager = GameManager.GetManagerClass<ResourceManager>();
        
        uiManager.deckScene = this;
       
        bool bisSet = false;
        prevButton.gameObject.SetActive(false);

        foreach (Transform tr in ListObject)
        {
            DeckList dl = tr.gameObject.GetComponent<DeckList>();
            dl.Initialize(bisSet,decklistIndex);
            deckList.Add(dl);
            decklistIndex++;
        }
        /*
        foreach(Transform tr in CardListObject)
        {
            DeckSettingHero dh = tr.gameObject.GetComponent<DeckSettingHero>();
            DeckSettingSpell ds = tr.gameObject.GetComponent<DeckSettingSpell>();

            heroList.Add(dh);
            spellList.Add(ds);
        }
        for(int i=0;i<spellList.Count;i++)
        {
            Debug.Log(spellList[i].gameObject.name);
        }
        */
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void Initialize()
    {
        deckDiscardUI.gameObject.SetActive(false);

        bool bisSet = characterManager.CheckCardListEmpty();
        
        CardData cd = null;      
        Sprite type = null;
        
        int deckLength = resourceManager.GetCardLength();
        deckLength = deckLength > heroList.Count ? heroList.Count : deckLength;

       
        for(int i=0;i<heroList.Count;i++)
        {
            heroList[i].Initialize(i,canvas,characterManager);
            spellList[i].Initialize(i, canvas, characterManager);
            heroList[i].gameObject.SetActive(false);
            spellList[i].gameObject.SetActive(false);
        }

        switch (bisSet)
        {
            case true:
                EmptySetter();
                break;
            case false:
                FullSetter();
                break;

        }
        #region 비어있을 때(덱을 최초로 생성 할 때)
        void EmptySetter()
        {           
            for (int i = 0; i < deckList.Count; i++)
            {
                deckList[i].changeSetting(null, 0, "");
            }

            for(int j =0; j < deckLength; j++)
            {
                cd = resourceManager.GetCardData(j);
                switch(cd.cardType)
                {
                    case CardData.CardType.Hero:
                        heroList[j].gameObject.SetActive(true);
                        heroList[j].SetCardInfo(cd.sprite, cd.CardCost, cd.CardPower, cd.CardBarrier, cd.CardName, cd.CardExplain, false);
                        break;

                    case CardData.CardType.Spell:
                        spellList[j].gameObject.SetActive(true);
                        type = resourceManager.GetSprite(cd.spellType.ToString());
                        spellList[j].SetCardInfo(cd.sprite, type, cd.CardCost, ref cd.DamageArea, cd.CardName, cd.CardExplain, false);
                        break;
                }
            }           
        }
        #endregion
        #region 차있을 때(덱을 변경할 때)
        void FullSetter()
        {
            bool bisExist = false;
            Sprite sprite = null;
            int cost = 0;
            string name = "";

            HashSet<int> comp = new HashSet<int>();
            
            for(int index = 0; index < characterManager.playerCards.Length;index++)
            {
                comp.Add(characterManager.playerCards[index].CardId);
            }

            for (int i = 0; i < characterManager.playerCards.Length; i++)
            {
                sprite = characterManager.playerCards[i].sprite;
                cost = characterManager.playerCards[i].CardCost;
                name = characterManager.playerCards[i].CardName;
                deckList[i].changeSetting(sprite, cost, name);
            }

            for (int i = 0; i < heroList.Count; i++)
            {
                heroList[i].gameObject.SetActive(false);
                spellList[i].gameObject.SetActive(false);
            }

            for (int j = 0; j < deckLength; j++)
            {
                cd = resourceManager.GetCardData(j);
                bisExist = comp.Contains(cd.CardId);
                switch (cd.cardType)
                {
                    case CardData.CardType.Hero:
                        heroList[j].gameObject.SetActive(true);
                        heroList[j].SetCardInfo(cd.sprite, cd.CardCost, cd.CardPower, cd.CardBarrier, cd.CardName, cd.CardExplain, bisExist);
                        break;

                    case CardData.CardType.Spell:
                        spellList[j].gameObject.SetActive(true);
                        type = resourceManager.GetSprite(cd.spellType.ToString());
                        spellList[j].SetCardInfo(cd.sprite,  type, cd.CardCost, ref cd.DamageArea, cd.CardName, cd.CardExplain, bisExist);
                        break;
                }
            }

        }
        #endregion
    }


    public void AdjustInfo()
    {     
        for(int i=0;i<deckList.Count;i++)
        {
            if (characterManager.playerCards[i].CardId == 0) {deckList[i].changeSetting(null, 0, "");}
            else deckList[i].changeSetting(characterManager.playerCards[i].sprite, characterManager.playerCards[i].CardCost, characterManager.playerCards[i].CardName);
        }

        CardData cd = null;
        bool bisExist = false;
        int deckLength = resourceManager.GetCardLength();
        deckLength = (deckLength - DeckIndex * 12) > heroList.Count ? 12 : deckLength - DeckIndex * 12;
        HashSet<int> comp = new HashSet<int>();

        for (int i = 0; i < characterManager.playerCards.Length; i++)
        {
            comp.Add(characterManager.playerCards[i].CardId);
        }

        for (int j = 0; j < deckLength; j++)
        {
            cd = resourceManager.GetCardData(DeckIndex *12 + j);
            bisExist = comp.Contains(cd.CardId);
            switch (cd.cardType)
            {
                case CardData.CardType.Hero:
                    heroList[j].SetCardInfo(bisExist);
                    break;

                case CardData.CardType.Spell:
                    spellList[j].SetCardInfo(bisExist);
                    break;
            }
        }
    }

    public void DisplayCard()
    {
        int deckLength = resourceManager.GetCardLength();      
        if (DeckIndex < 0 || DeckIndex > deckLength / 12) return;        
        deckLength = (deckLength - DeckIndex *12)  > heroList.Count ? heroList.Count : deckLength - DeckIndex * 12;

        

        CardData cd = null;    
        Sprite type = null;
        bool bisExist = false;

        HashSet<int> comp = new HashSet<int>();

        for (int index = 0; index < characterManager.playerCards.Length; index++)
        {
            comp.Add(characterManager.playerCards[index].CardId);
        }

        for(int i=0; i<12; i++)
        {
            heroList[i].gameObject.SetActive(false);
            spellList[i].gameObject.SetActive(false); 
        }


        for (int j = 0; j < deckLength; j++)
        {
            cd = resourceManager.GetCardData(DeckIndex * 12 + j);
            bisExist = comp.Contains(cd.CardId);
          
            switch (cd.cardType)
            {
                case CardData.CardType.Hero:
                    heroList[j].gameObject.SetActive(true);
                    heroList[j].SetCardInfo(cd.sprite, cd.CardCost, cd.CardPower, cd.CardBarrier, cd.CardName, cd.CardExplain, bisExist);
                    break;

                case CardData.CardType.Spell:
                    spellList[j].gameObject.SetActive(true);                 
                    type = resourceManager.GetSprite(cd.spellType.ToString());
                    
                    spellList[j].SetCardInfo(cd.sprite,  type, cd.CardCost, ref cd.DamageArea, cd.CardName, cd.CardExplain, bisExist);
                    break;
            }
        }
    }





    public void ListChangeButton(int type)
    {       
        switch (type)
        {
            case 0:
                DeckIndex -= 1;
                nextButton.gameObject.SetActive(true);
                if (DeckIndex < 0) DeckIndex = 0; 
                DisplayCard();
                break;
                
            case 1:
                DeckIndex += 1;
                prevButton.gameObject.SetActive(true);
                if (DeckIndex > resourceManager.GetCardLength() / 12)  DeckIndex = resourceManager.GetCardLength() / 12;
                DisplayCard();
                break;
        }

        if (DeckIndex <= 0) prevButton.gameObject.SetActive(false);
        if (DeckIndex == resourceManager.GetCardLength() / 12) nextButton.gameObject.SetActive(false);

    }

    public void onClickListButton(int index)
    {
        deckDiscardUI.index = index;
        CardData cd = characterManager.playerCards[index];
        Sprite type = null;
        if (cd.CardId == 0) return;
        deckDiscardUI.gameObject.SetActive(true);
        switch (cd.cardType)
        {
            case CardData.CardType.Hero:
                deckDiscardUI.DisplayCard(cd.sprite,cd.CardName,cd.CardExplain,cd.CardCost,cd.CardPower,cd.CardBarrier);
                break;
            case CardData.CardType.Spell:
                deckDiscardUI.DisplayCard(cd.sprite,type,cd.CardName,cd.CardExplain,cd.CardCost, ref cd.DamageArea);
                break;        
        }       
    }
   
    public void onClickQuitButton()
    {
        deckDiscardUI.gameObject.SetActive(false);
    }

    public void onClickDecisionButton()
    {
        if (!characterManager.CheckDeckFull()) return;
        uiManager.mainMenu.InitializeSlot();
        gameObject.SetActive(false);
    }
}
