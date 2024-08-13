using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainSlot : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TextMeshProUGUI Cost;
    [SerializeField] private TextMeshProUGUI Name;

    public void DisplaySlot(Sprite face, int cost, string name)
    { 
      if (face == null)
        {
            gameObject.SetActive(false);
        }
      cardImage.sprite = face;
      Cost.text = string.Format("{0}",cost);
      Name.text = name;
    }



}
