using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeckSettingHero : CardUIBase
{
    #region Component
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI barrierText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private TextMeshProUGUI dragnameText;
    [SerializeField] private TextMeshProUGUI dragExplainText;
    [SerializeField] private TextMeshProUGUI dragPowerText;
    [SerializeField] private TextMeshProUGUI dragBarrierText;
    [SerializeField] private TextMeshProUGUI dragCostText;

    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackImage;
                     
    [SerializeField] private Image dragImage;
    [SerializeField] private Image dragBackImage;


    #endregion


    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (bisFixed) return;
        if (!bisDown)
        {
            OriginCard.gameObject.SetActive(false);
            DragCard.anchoredPosition = DragOriginPos;
            DragCard.gameObject.SetActive(true);
            DragCard.transform.parent = canvas.transform;
            transform.localScale = new Vector3(1, 1, 1);
            bisDown = true;
        }       
    }

    public override void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);
    }



    public void OnPointerUp(PointerEventData pointerEventData)
    {       
      
        if (!bisDown) return;
        bool bisfull = characterManager.CheckDeckFull();
        if (pointerEventData.position.x > Screen.width || pointerEventData.position.y > Screen.height || pointerEventData.position.x < 0 || pointerEventData.position.y < 0) { BackToOrigin(); return; }

        if (pointerEventData.pointerCurrentRaycast.gameObject.CompareTag("DeckList"))
        {
            switch (bisfull)
            {
                case true:
                    BackToOrigin();
                   
                    break;

                case false:
                    OnFixedCard(true);
                    
                    characterManager.AddPlayerCard(index);
                   
                    break;                       
            }
        }
        else
        {
            BackToOrigin();
        }
    }

    private void OnFixedCard(bool bisUsed)
    {
        DragCard.transform.parent = this.transform;
        DragCard.transform.localScale = new Vector3(1, 1, 1);
        DragCard.anchoredPosition = DragOriginPos;
        
        bisDown = false;
        OriginCard.gameObject.SetActive(true);
        DragCard.gameObject.SetActive(false);

        bisFixed = bisUsed;

        switch(bisUsed)
        {
            case true :
              
                costText.color = Color.gray;
                powerText.color = Color.gray;
                barrierText.color = Color.gray;

                cardImage.color = Color.gray;
                cardBackImage.color = Color.gray;

                break;

            case false :

                costText.color = Color.white;
                powerText.color = Color.white;
                barrierText.color = Color.white;

                cardImage.color = Color.white;
                cardBackImage.color = Color.white;
                
                break;

        }

    }


    public void SetCardInfo(Sprite face ,  int cost, int power, int barrier, string name, string explain, bool bisUsed)
    {
        cardImage.sprite = face;
        dragImage.sprite = face;

        costText.text = string.Format("{0}",cost);
        dragCostText.text = string.Format("{0}",cost);

        powerText.text = string.Format("{0}",power);
        dragPowerText.text = string.Format("{0}",power);

        barrierText.text = string.Format("{0}",barrier);
        dragBarrierText.text = string.Format("{0}",barrier);

        nameText.text = name;
        dragnameText.text = name;

        explainText.text = explain;
        dragExplainText.text = explain; ;

        OnFixedCard(bisUsed);
    }

    public void SetCardInfo(bool bisUsed)
    {
        OnFixedCard(bisUsed);
    }

    public new void Initialize(int ind, Canvas canvas, CharacterManager ch)
    {
        base.Initialize(ind, canvas, ch);

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);


        EventTrigger.Entry entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerUp);

        DragCard.gameObject.SetActive(false);
    }

}
