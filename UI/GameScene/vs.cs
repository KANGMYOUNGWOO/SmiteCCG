using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class vs : MonoBehaviour
{
    [SerializeField] private RectTransform player;
    [SerializeField] private RectTransform Enemy;
    [SerializeField] private TextMeshProUGUI playerText;
    [SerializeField] private TextMeshProUGUI enemyText;
    [SerializeField] private Image Image;

    private CharacterManager characterManager;


    private void Awake()
    {
        characterManager = GameManager.GetManagerClass<CharacterManager>();
    }

    public void startvs(string text)
    {
        playerText.text = text;
        player.DOAnchorPos(new Vector2(-352f,-21f),1.0f).SetEase(Ease.OutBounce).From(new Vector2(-1291,-21));
        Enemy.DOAnchorPos(new Vector2(352f, -21f), 1.0f).SetEase(Ease.OutBounce).From(new Vector2(1291, -21)).OnComplete(() => completevs());
    }

    private void completevs()
    {
        playerText.gameObject.SetActive(false);
        enemyText.gameObject.SetActive(false);
        Image.DOFade(0,1.0f).OnComplete(() => { gameObject.SetActive(false); characterManager.GameStart(); }).SetDelay(1.0f);     
        player.DOAnchorPos(new Vector2(-735f, 450f), 1.0f).SetEase(Ease.OutFlash);
        Enemy.DOAnchorPos(new Vector2(647.7f,450f),1.0f).SetEase(Ease.OutFlash);
        player.DOSizeDelta(new Vector2(214f, 146f), 1.0f).SetEase(Ease.OutFlash).OnComplete(()=>player.gameObject.SetActive(false));
        Enemy.DOSizeDelta(new Vector2(214f,146f),1.0f).SetEase(Ease.OutFlash).OnComplete(()=>Enemy.gameObject.SetActive(false));
    }
}
