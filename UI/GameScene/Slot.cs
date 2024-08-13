using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FronkonGames.SpritesMojo;
using System;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    private Material redMat;
    private Material greenMat;
    private Material blueMat;
    private Material normalMat;
    private SpriteRenderer spriteRenderer = null;
    private CharacterManager characterManager = null;
    private SpriteRenderer heart;
    private TextMeshProUGUI text;

    private void Awake()
    {
        redMat = Hologram.CreateMaterial();
        greenMat = Hologram.CreateMaterial(); 
        blueMat = Hologram.CreateMaterial();
        normalMat = Hologram.CreateMaterial();

        Initialize(7.1f,0.41f,10f,0f,3.8f,0,"#FF0025",redMat);
        Initialize(7.1f,0.41f,5f,0f,3.8f,0, "#69FFC5", greenMat);
        Initialize(7.1f,0.41f,10f,0f,3.8f,0, "#009FFF", blueMat);

        SpriteMojo.Amount.Set(normalMat, 0);
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = normalMat;
        //heart = GetComponentInChildren<SpriteRenderer>();
        //heart.gameObject.SetActive(false);
        //text=GetComponentInChildren<TextMeshProUGUI>();
        //text.gameObject.SetActive(false);
        //characterManager = GameManager.GetManagerClass<CharacterManager>();

    }

    private void Initialize(float dis ,float bs , float speed ,float ss,float sc , float sspeed ,string hash ,Material mat)
    {
        Hologram.Distortion.Set(mat,dis);
        Hologram.BlinkStrength.Set(mat,bs);
        Hologram.BlinkSpeed.Set(mat,speed);
        Hologram.ScanlineStrength.Set(mat,ss);
        Hologram.ScanlineCount.Set(mat,sc);
        Hologram.ScanlineSpeed.Set(mat,sspeed);

        Color color;
        ColorUtility.TryParseHtmlString(hash, out color);

        Hologram.Tint.Set(mat,color);
    }

    public void ChangeColor(int color)
    {
        switch(color)
        {
            case 0:
                spriteRenderer.material = normalMat;
                break;

            case 1:
                spriteRenderer.material = redMat;
                break;

            case 2:
                spriteRenderer.material = greenMat;
                break;
            case 3:
                spriteRenderer.material = normalMat;
                break;
        }

    }

    public void ChangeColor(Sprite sprite)
    {
        spriteRenderer.material = normalMat;
        spriteRenderer.sprite = sprite;
    }

    private void Start()
    {
        
    }

}
