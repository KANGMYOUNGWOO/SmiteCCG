using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;

public class Login : MonoBehaviour
{
    UIManager uIManager;
    CharacterManager characterManager;

    private void Awake()
    {
        uIManager = GameManager.GetManagerClass<UIManager>();
        characterManager = GameManager.GetManagerClass<CharacterManager>();   
    }


    [SerializeField] private TMP_InputField ID;
    [SerializeField] private TMP_InputField PW;
    [SerializeField] private VideoPlayer vid;

    public void OnClickLogin()
    {
        if (ID.text.Length > 0 && PW.text.Length > 0)
        {
            characterManager.ID = ID.text;
            gameObject.SetActive(false); 
        }
    }


    public void StartLogin()
    {
        vid.Play();
    }


}
