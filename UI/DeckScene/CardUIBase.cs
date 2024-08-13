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

    protected bool bisDown = false;   // ī�� �������� ��Ȯ�ϰ� �ϱ� ���� �����ϴ� ����    
    protected bool bisFixed = false;  // �̹� �� ����Ʈ�� ���Ե� ī��� ������ �� �����Ѵ�.
    protected int index;              // �ڽ��� ��� ° ī������ ���
    protected Vector2 DragOriginPos;  // �巡�� ī��(�巡�� �� �� �÷��̾��� ���콺�� ���� ������ ī��)�� ���� ��ġ�� ���ư� �� �ְ� ��ġ ���

    /*    
                         �⺻ ����

      * �� GameObject�� Card�� OriginCard�� DragCard�� �ڽ����� ������. 
      * �ʱ�ȭ �Ǿ��� �� OriginCard�� Ȱ��, DragCard�� ��Ȱ��ȭ �� 
      * Card�� Ŭ���ϸ� -> OriginCard�� ��Ȱ��ȭ, DragCard Ȱ��ȭ  : bisdown = true ��
      * DragCard  -> Card�� Ŭ���� �巡�� �ϸ� �θ� MainCanvas�� ����Ǿ� ���콺�� ��ġ�� ����ٴѴ�. : bisdown == true 
      * DragCard�� ����ϸ�, � ������Ʈ�� ����߳Ŀ� ���� �ٸ� �׼� �۵�
      * case : DeckList -> �÷��̾��� ���� Ǯ�� �ƴϸ�, ī�带 �����. Ǯ�̸� ? ��ü��
      * case : DeckFrame-> �÷��̾��� ���� Ǯ�� �ƴϸ�, ī�带 �����. Ǯ�̸� ? ��ȿ
      * case : None     -> ��ȿ�� �� ������ġ�� ī�� ����->  OriginCard�� ��Ȱ��, DragCard�� Ȱ�� : bisdown = false ��
      
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
            //ī�尡 ���콺 ��ġ�� ���󰡰� �ϴµ� ȭ�� ������ ����ϰ� ��
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
