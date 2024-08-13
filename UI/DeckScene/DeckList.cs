using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckList : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI CostText;

    private int index = 0;

    public void Initialize(bool bisSet , int ind)
    {      
        nameText = GetComponentInChildren<TextMeshProUGUI>();
        index = ind;
        //gameObject.SetActive(bisSet);
    }

    public void changeSetting(Sprite sprite, int cost, string name)
    {
        if (sprite == null)
        {
            cardImage.color = new Color(0, 0, 0, 0);
            nameText.text = "";
            CostText.text = "";
        }
        else
        {            
            cardImage.sprite = sprite;
            cardImage.color = Color.white;
            nameText.text = name;
            CostText.text = string.Format("{0}",cost);
        }       
    }
}
