using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;


public class CardUIBase : MonoBehaviour,IDragHandler, IEndDragHandler
{
    protected CharacterManager characterManager;

    #region Component
    [SerializeField] protected RectTransform OriginCard;
    [SerializeField] protected RectTransform DragCard;
                 
    public Canvas canvas { get; private set; }
    

    #endregion

    protected bool bisDown = false;   // 카드 움직임을 정확하게 하기 위해 제어하는 변수    
    protected bool bisFixed = false;  // 이미 덱 리스트에 포함된 카드면 움직일 수 없게한다.
    protected int index;              // 자신이 몇번 째 카드인지 기억
    protected Vector2 DragOriginPos;  // 드래그 카드(드래그 할 때 플레이어의 마우스를 따라 움직일 카드)가 원래 위치로 돌아갈 수 있게 위치 기억

    /*    
                         기본 설계

      * 빈 GameObject인 Card는 OriginCard와 DragCard를 자식으로 가진다. 
      * 초기화 되었을 떄 OriginCard는 활성, DragCard는 비활성화 됨 
      * Card를 클릭하면 -> OriginCard를 비활성화, DragCard 활성화  : bisdown = true 됨
      * DragCard  -> Card를 클릭해 드래그 하면 부모가 MainCanvas로 변경되어 마우스의 위치를 따라다닌다. : bisdown == true 
      * DragCard를 드롭하면, 어떤 오브젝트에 드롭했냐에 따라 다른 액션 작동
      * case : DeckList -> 플레이어의 덱이 풀이 아니면, 카드를 등록함. 풀이면 ? 교체함
      * case : DeckFrame-> 플레이어의 덱이 풀이 아니면, 카드를 등록함. 풀이면 ? 무효
      * case : None     -> 무효된 후 원래위치로 카드 보냄->  OriginCard는 비활성, DragCard는 활성 : bisdown = false 됨
      
   */


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
    }

    public virtual void OnDrag(PointerEventData data)
    {
        if (bisFixed) return;
        if (bisDown)
        {
            Vector2 localPosition;
            //카드가 마우스 위치를 따라가게 하는데 화면 비율을 고려하게 함
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, data.position, canvas.worldCamera, out localPosition);
            DragCard.anchoredPosition = localPosition;
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
       
    }


    public void Initialize(int ind, Canvas canvas, CharacterManager ch)
    {
        bisDown = false;
        DragOriginPos = DragCard.anchoredPosition;
        this.canvas = canvas;
        characterManager = ch;
        this.index = ind;
              
    }


    public  void SetCardInfo() 
    {
        
    }

}
