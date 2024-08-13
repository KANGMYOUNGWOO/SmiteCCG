using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour, IManager
{
    public GameManager gameManager   { get { return GameManager.gameManager; } }   
    public MainMenu mainMenu         { get; set; }
    public LoadingScene loadingScene { get; set; }
    public Login login               { get; set; }
    public DeckScene deckScene       { get; set; }
    public GameScene gameScene       { get; set; }

    private float temp = 0;

    private void SetResolution()
    {
        int setWidth = 1600;
        int setHeight = 1080;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true);

        if((float)setWidth / setHeight < (float)deviceHeight/deviceWidth)
        {
            float newWidth = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = ((float)deviceWidth/deviceHeight) / ((float)setWidth/setWidth);
            Camera.main.rect = new Rect(0f,(1f-newHeight) / 2f, 1f, newHeight);
        }
    }

    public void showLoading()
    {      
        if(loadingScene == null)
        loadingScene = GameObject.Find("LoadScene").GetComponent<LoadingScene>();
        loadingScene.Loading();
    }

    private void Awake()
    {
        SetResolution();
       
    }

    public void InitializeAfterLoad()
    {
        deckScene.Initialize();
        mainMenu.InitializeSlot();
        gameScene.Initialize();
    }

}
