using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DeckSettingSpell : CardUIBase
{
    #region Component;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private TextMeshProUGUI dragName;
    [SerializeField] private TextMeshProUGUI dragExplain;
    [SerializeField] private TextMeshProUGUI dragCostText;

    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackImage;
    [SerializeField] private Image cardTypeImage;
    [SerializeField] private List<Image> ranges = new List<Image>();

    [SerializeField] private Image dragImage;
    [SerializeField] private Image dragBackImage;
    [SerializeField] private Image dragTypeImage;
    [SerializeField] private List<Image> dragranges = new List<Image>();

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

        switch (bisUsed)
        {
            case true:

                costText.color = Color.gray;
              
                cardImage.color = Color.gray;
                cardBackImage.color = Color.gray;

                break;

            case false:

                costText.color = Color.white;
             
                cardImage.color = Color.white;
                cardBackImage.color = Color.white;

                break;

        }
    }

    public void SetCardInfo(Sprite face, Sprite type , int cost, ref int[] array, string name, string explain, bool bisUsed)
    {
        Color gray = new Color();
        Color blue = new Color();
        Color red = new Color();

        

        ColorUtility.TryParseHtmlString("#1B1C1B", out gray);
        ColorUtility.TryParseHtmlString("#C6002E", out red);
        ColorUtility.TryParseHtmlString("#000FBE", out blue);

        cardImage.sprite = face;
        dragImage.sprite = face;
     
        nameText.text = name;
        dragName.text = name;

        cardTypeImage.sprite = type;
        dragTypeImage.sprite = type;

        costText.text = string.Format("{0}", cost);
        dragCostText.text = string.Format("{0}", cost);

        explainText.text = explain;
        dragExplain.text = explain; ;

        for(int i=0;i<array.Length;i++)
        {
            switch(array[i])
            {
                case 0:
                    ranges[i].color = gray;
                    dragranges[i].color = gray;
                    break;
                case 1:
                    ranges[i].color = red;
                    dragranges[i].color = red;
                    break;
                case 2:
                    ranges[i].color = blue;
                    dragranges[i].color = blue;
                    break;
            }
        }

        OnFixedCard(bisUsed);


    }

    public void SetCardInfo(bool bisExist)
    {
        OnFixedCard(bisExist);
    }

    public new void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);
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
