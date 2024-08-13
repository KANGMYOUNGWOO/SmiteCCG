using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.UIElements;

public class ResourceManager : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    private List<CardData> cardDatas = new List<CardData>();   
    private Dictionary<string, Sprite> CardOptionSprites = new Dictionary<string, Sprite>();

    private UIManager uiManager;
    private CharacterManager characterManager;

    private int LoadIndex = 0;
    private int fullLoadIndex = 2;

    

    //public Dictionary<string,Sprite> sprites = new Dictionary<string,Sprite>();
    public CardData GetCardData(string name)
    {
        for(int i=0;i<cardDatas.Count;i++)
        {
            if (cardDatas[i].CardName == name)
                return cardDatas[i];          
        }
        Debug.LogError("해당 이름의 애셋을 찾을 수 없습니다");
        return null;
    }
    
    public CardData GetCardData(int index)
    {
      if(index >= cardDatas.Count)
        {
            Debug.LogError("접근하려는 인덱스가 배열의 크기 이상입니다.");
            return null;
        }

      return cardDatas[index];      
    }

    public Sprite GetSprite(string name)
    {
        Sprite s = null;
       
        if (CardOptionSprites.ContainsKey(name)) s = CardOptionSprites[name];
        return s;
    }

    public int GetCardLength()
    {
        return cardDatas.Count;
    }


    IEnumerator WaitLoad(AsyncOperationHandle<GameObject> op)
    {
        while(op.IsDone)
        {
            yield return op;
        }
    }

    private void OnDownlodComplete(Sprite obj)
    {
        CardOptionSprites.Add(obj.name,obj);
      
    }

    private void OnScDownloadComplete(CardData card)
    {
        cardDatas.Add(card);
    }

    
    private void LoadData (string code) 
    {
        Addressables.LoadAssetsAsync<CardData>(code, OnScDownloadComplete).Completed += (handle) => {
            if (handle.IsDone) LoadEnd();            
        };
    }


    private void LoadSprite(string code)
    {
        Addressables.LoadAssetsAsync<Sprite>(code, OnDownlodComplete).Completed += (handle) =>{
            if (handle.IsDone) LoadEnd();
        };
    }


    private void LoadEnd()
    {
        LoadIndex += 1;
        if (LoadIndex >= fullLoadIndex) 
        {
            LoadIndex = -999;
            uiManager.showLoading();
            uiManager.InitializeAfterLoad();
        }
    }

    public void SpawnModel(string code, Vector3 pos)
    {        
        AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(code, pos, Quaternion.identity);                     
        StartCoroutine(WaitForSpawnComplete(handle,code));       
    }

    IEnumerator WaitForSpawnComplete(AsyncOperationHandle<GameObject> handle , string code)
    {
        while (handle.IsDone == false)
        {
            yield return handle;
        }

        OnSpawnComplete(handle,code);
    }

    void OnSpawnComplete (AsyncOperationHandle<GameObject> handle, string code)
    {
        if (code == "cursorLight") characterManager.gamescene.GetcursorLight(handle.Result);
        else if (code == "Spawn") characterManager.gamescene.GetSpawn(handle.Result);
        else if (code == "Charge") characterManager.gamescene.GetCharge(handle.Result);
    }



    



    private void ReleaseObject(GameObject rel)
    {
        Addressables.ReleaseInstance(rel);
    }


    private void Awake()
    {
        uiManager = GameManager.GetManagerClass<UIManager>();
        characterManager = GameManager.GetManagerClass<CharacterManager>();
        LoadData("CardData");
        LoadSprite("ImageAsset");

    }





}
