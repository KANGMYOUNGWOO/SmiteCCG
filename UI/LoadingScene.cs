using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using FronkonGames.SpritesMojo;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]private Image LoadBar;
    [SerializeField]private Image BackGround;

    private Material dissolveMat;
    private UIManager uiManager;
    private TextMeshProUGUI loadText;

    public void Loading()
    {
        
   
        if(LoadBar == null || BackGround == null) 
        { 
            Debug.LogError("오브젝트를 찾을수 없습니다.");
            return;
        }
        loadText = GetComponentInChildren<TextMeshProUGUI>();

        LoadBar.fillAmount = 0;
        LoadBar.DOFillAmount(1, 2f).From(0).SetEase(Ease.Flash).OnComplete(()=> StartCoroutine(dissolveFunc()));
        dissolveMat = Dissolve.CreateMaterial();
        Dissolve.Slide.Set(dissolveMat, 0);
        Dissolve.Shape.Set(dissolveMat, DissolveShape.LuminousFrame_1);
        BackGround.material = dissolveMat;
       

      
    }

    IEnumerator dissolveFunc()
    {
        WaitForSeconds wait = new WaitForSeconds(0.001f);
        float dissolveFloat = 0f;
        LoadBar.gameObject.SetActive(false);
        loadText.gameObject.SetActive(false);

        while (dissolveFloat < 1)
        {           
            dissolveFloat += 0.01f;
            Dissolve.Slide.Set(dissolveMat, dissolveFloat);
            yield return wait;
        }

        gameObject.SetActive(false);
    }


}
