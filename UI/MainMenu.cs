using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private UIManager uiManager;
    private CharacterManager characterManager;
    [SerializeField] private List<MainSlot> mainSLots = new List<MainSlot>();


    private void Awake()
    {
       uiManager = GameManager.GetManagerClass<UIManager>();
       characterManager = GameManager.GetManagerClass<CharacterManager>();
       uiManager.mainMenu = this;
    }

    public void ChangeDeck()
    {       
        uiManager.deckScene.gameObject.SetActive(true);
    }

    public void InitializeSlot()
    {
        if (characterManager.CheckCardListEmpty())
        {
            for(int i=0;i<mainSLots.Count;i++)
            {
                mainSLots[i].DisplaySlot(null,0,"");
            }
        }
        else
        {            
            for (int i = 0; i < mainSLots.Count; i++)
            {
                mainSLots[i].gameObject.SetActive(true);
                mainSLots[i].DisplaySlot(characterManager.playerCards[i].sprite, characterManager.playerCards[i].CardCost, characterManager.playerCards[i].CardName); 
            }
        }
    }

   

    public void GameStart()
    {
        if (!characterManager.CheckDeckFull()) return;

        uiManager.gameScene.StartGame(characterManager.ID);
        gameObject.SetActive(false);
    }
}
