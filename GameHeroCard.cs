using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameHeroCard : CardUIBase
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
            characterManager.SetCurrentCard(index);
        }
    }

    public override void OnDrag(PointerEventData data)
    {
        base.OnDrag(data);
       

        if (data.position.x > Screen.width || data.position.y > Screen.height || data.position.x < 0 || data.position.y < 0) { BackToOrigin(); return; }
        if (data.pointerCurrentRaycast.gameObject == null) return;

        if (data.pointerCurrentRaycast.gameObject.CompareTag("GameSlot"))
        {
            //Debug.Log(data.pointerCurrentRaycast.gameObject.name);
            characterManager.ShowGUI(int.Parse(data.pointerCurrentRaycast.gameObject.name));
        }
        //if (data.pointerCurrentRaycast.gameObject.CompareTag("GameSlot"))
        //{
        // characterManager.ShowGUI(int.Parse(data.pointerCurrentRaycast.gameObject.name));          
        //}
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        int data = 0;
        if (!characterManager.CheckMana()) { BackToOrigin(); return; }
        if (pointerEventData.position.x > Screen.width || pointerEventData.position.y > Screen.height || pointerEventData.position.x < 0 || pointerEventData.position.y < 0) { BackToOrigin(); return; }
        if (pointerEventData.pointerCurrentRaycast.gameObject == null) { BackToOrigin(); return; }
        if (pointerEventData.pointerCurrentRaycast.gameObject.CompareTag("GameSlot"))
        {        
            data = int.Parse(pointerEventData.pointerCurrentRaycast.gameObject.gameObject.name);
            if (characterManager.CheckGameCard(data))
            {
                DragCard.transform.parent = this.transform;
                DragCard.transform.localScale = new Vector3(1, 1, 1);
                bisDown = false;
                DragCard.gameObject.SetActive(false);
                OriginCard.gameObject.SetActive(true);
                characterManager.SetGameCard(data);
                gameObject.SetActive(false);
            }
            else
            {
                BackToOrigin();
            }
        }
        else BackToOrigin();
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

    public void SetCardInfo(Sprite face, int cost, int power, int barrier, string name, string explain)
    {
        if (face == null) { gameObject.SetActive(false);  return; }
        cardImage.sprite = face;
        dragImage.sprite = face;

        costText.text = string.Format("{0}", cost);
        dragCostText.text = string.Format("{0}", cost);

        powerText.text = string.Format("{0}", power);
        dragPowerText.text = string.Format("{0}", power);

        barrierText.text = string.Format("{0}", barrier);
        dragBarrierText.text = string.Format("{0}", barrier);

        nameText.text = name;
        dragnameText.text = name;

        explainText.text = explain;
        dragExplainText.text = explain; ;        
    }
}
