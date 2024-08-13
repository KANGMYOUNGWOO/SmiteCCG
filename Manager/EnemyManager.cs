using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyManager : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    private CardData[] enemyDeck = new CardData[10];
    private CardData[] enemyHand = new CardData[7];

    private CharacterManager characterManager;
    private ResourceManager resourceManager;

    public void TurnMethod()
    {
        if (characterManager.Gods.Count > 0)
        {
            SetEmpty();
        }
        else
        {
            Approach();
        }

        void SetEmpty()
        {
            for (int i = 0; i < 5; i++)
            {
                for(int j=0;j<7;j++)
                {
                    if(characterManager.FactionArray[j, i] == 0)
                    {
                        characterManager.FactionArray[j, i] = 2;
                        break;
                    }
                }
            }
        }

        void Approach()
        {
            int i = 0;
            int target_x = 0;
            int target_y = 0;
            while(i < characterManager.Gods.Count)
            {
                target_x  = characterManager.Gods[i].Positon / 10;
                target_y  = characterManager.Gods[i].Positon % 10;
                
                if (target_x > 0 && characterManager.FactionArray[target_x-1,target_y] == 0)
                {
                    characterManager.FactionArray[target_x - 1, target_y] = 2;
                    break;
                }

                if(target_x <6 && characterManager.FactionArray[target_x+1, target_y] == 0)
                {
                    characterManager.FactionArray[target_x + 1, target_y] = 2;
                    break;
                }

                if (target_y > 0 && characterManager.FactionArray[target_x, target_y-1] == 0)
                {
                    characterManager.FactionArray[target_x , target_y-1] = 2;
                    break;
                }

                if (target_y < 4 && characterManager.FactionArray[target_x, target_y + 1] == 0)
                {
                    characterManager.FactionArray[target_x, target_y + 1] = 2;
                    break;
                }


            }
        }

    }



}
