using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GameCard : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform OriginCard;
    [SerializeField] private RectTransform DragCard;

    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardBackImage;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI explainText;
    [SerializeField] private TextMeshProUGUI powerText;
    [SerializeField] private TextMeshProUGUI barrierText;
    [SerializeField] private TextMeshProUGUI costText;

    [SerializeField] private Image dragImage;
    [SerializeField] private Image dragBackImage;

    [SerializeField] private TextMeshProUGUI dragnameText;
    [SerializeField] private TextMeshProUGUI dragExplainText;
    [SerializeField] private TextMeshProUGUI dragPowerText;
    [SerializeField] private TextMeshProUGUI dragBarrierText;
    [SerializeField] private TextMeshProUGUI dragCostText;
    private bool bisDown = false;
    private bool bisEndDraw;
    public Canvas canvas;
    private int index;
    private Vector2 DragOriginPos;
    private CharacterManager characterManager;

    public void Initialize(int ind, Canvas canvas, CharacterManager ch)
    {
        bisDown = false;
        DragOriginPos = DragCard.anchoredPosition;
        this.canvas = canvas;
        characterManager = ch;
        this.index = ind;


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

    public void CardSet(Sprite card, Sprite back, string name, int cost , int power, int barrier, int option)
    {
        /*
        bisEndDraw = false;
        cardImage.sprite = card;
        dragImage.sprite = card;
        cardBackImage.sprite = back;
        dragBackImage.sprite = back;
        nameText.text = name; ;
        dragnameText.text = name;
        costText.text = string.Format("{0}", cost);
        dragCostText.text = string.Format("{0", cost);
        powerText.text = string.Format("{0}", power);
        dragPowerText.text = string.Format("{0}", power);
        barrierText.text = string.Format("{0}", barrier);
        dragBarrierText.text = string.Format("{0}", barrier);

        OriginCard.gameObject.SetActive(false);
        DragCard.gameObject.SetActive(true);
        DragOriginPos = DragCard.anchoredPosition;
        DragCard.transform.localScale = new Vector3(1, 1, 1);
        DragCard.DOAnchorPos(DragOriginPos, 0.3f).SetDelay(2 * (option + 0.1f) + index * 0.1f).OnComplete(EndDraw).From(new Vector2(1136, DragOriginPos.y)).SetEase(Ease.Flash);
        */
    }



    public void OnDrag(PointerEventData data)
    {
        if (bisDown)
        {
            Vector2 localPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, canvas.worldCamera, out localPosition);
            DragCard.anchoredPosition = localPosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;
            if (Physics.Raycast(ray, out hitData, 10))
            {
                if (hitData.collider.gameObject.CompareTag("Slot")) characterManager.ShowGUI(int.Parse(hitData.collider.gameObject.name));

                // The Ray hit something less than 10 Units away,// It was on a certain Layer// But it wasn't a Trigger Collider
            }

        }
    }
    public void OnEndDrag(PointerEventData data)
    {

    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        if (!bisEndDraw) return;
        //bisDown = true;
        if (!bisDown)
        {

            OriginCard.gameObject.SetActive(false);
            DragCard.anchoredPosition = DragOriginPos;
            DragCard.gameObject.SetActive(true);
            DragCard.transform.parent = canvas.transform;
            transform.localScale = new Vector3(1, 1, 1);
            //isActive = false;
            bisDown = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 10))
            {
                if (hitData.collider.gameObject.CompareTag("Slot")) Debug.Log(gameObject.name);

                // The Ray hit something less than 10 Units away,// It was on a certain Layer// But it wasn't a Trigger Collider
            }

        }

    }
    public void OnPointerUp(PointerEventData pointerEventData)
    {
        if (!bisDown) return;


        if (pointerEventData.pointerCurrentRaycast.gameObject.CompareTag("DeckList"))
        {
            DragCard.transform.parent = this.transform;
            DragCard.transform.localScale = new Vector3(1, 1, 1);
            DragCard.anchoredPosition = DragOriginPos;
            DragCard.gameObject.SetActive(false);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitData;

            if (Physics.Raycast(ray, out hitData, 10))
            {
                if (hitData.collider.gameObject.CompareTag("Slot")) Debug.Log(gameObject.name);
                    
                // The Ray hit something less than 10 Units away,// It was on a certain Layer// But it wasn't a Trigger Collider
            }
        


            int Afterslotnum = int.Parse(pointerEventData.pointerCurrentRaycast.gameObject.name);
            //characterManager.SetCardList(index, Afterslotnum);
            gameObject.SetActive(false);
            //int.Parse(gameObject.name), Afterslotnum
            //logicManager.ButtonAction(type, index);
            //logicManager
        }
        else if (pointerEventData.pointerCurrentRaycast.gameObject.CompareTag("DeckFrame"))
        {

            DragCard.transform.parent = this.transform;
            DragCard.transform.localScale = new Vector3(1, 1, 1);
            DragCard.anchoredPosition = DragOriginPos;
            //characterManager.SetCardList(index);
            DragCard.gameObject.SetActive(false);
        }
        else BackToOrigin();

    }

    public void BackToOrigin()
    {
        DragCard.transform.parent = this.transform;
        DragCard.transform.localScale = new Vector3(1, 1, 1);
        DragCard.DOAnchorPos(DragOriginPos, 0.4f).SetEase(Ease.InOutCirc).OnComplete(SetActiveOrigin);
    }

    private void SetActiveOrigin()
    {
        bisDown = false;
        OriginCard.gameObject.SetActive(true);
        DragCard.gameObject.SetActive(false);
        // Cost.gameObject.SetActive(true);
        //if (biscost) CostText.gameObject.SetActive(true);

    }
    private void EndDraw()
    {
       

        DragCard.anchoredPosition = DragOriginPos;
        OriginCard.gameObject.SetActive(true);
        DragCard.gameObject.SetActive(false);
        cardImage.DOFade(1, 0.01f);
       
        nameText.gameObject.SetActive(true);

        //CardExplain.gameObject.SetActive(true);
        bisEndDraw = true;
    }
}
