using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeartUI : MonoBehaviour
{
    [SerializeField] Image heart;
    [SerializeField] Image barrier;
    [SerializeField] TextMeshProUGUI heartText;
    [SerializeField] TextMeshProUGUI barrierText;

    public void SetUI(int heart, int barrier)
    {
       this.heart.gameObject.SetActive(heart > 0);
       this.heartText.gameObject.SetActive(heart > 0);
        heartText.text = string.Format("{0}",heart);
        this.barrier.gameObject.SetActive(barrier > 0);
        this.barrierText.gameObject.SetActive(barrier > 0);
        barrierText.text = string.Format("{0}",barrier);
    }
}
