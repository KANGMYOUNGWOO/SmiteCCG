using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class DeckDiscardUI : MonoBehaviour
{
    UIManager uiManager = null;
    CharacterManager characterManager = null;

    [SerializeField] private GameObject Hero;
    [SerializeField] private GameObject Spell;

    [SerializeField] private Image heroImage;

    [SerializeField] private TextMeshProUGUI heroName;
    [SerializeField] private TextMeshProUGUI heroExplain;
    [SerializeField] private TextMeshProUGUI heroCost;
    [SerializeField] private TextMeshProUGUI heroPower;
    [SerializeField] private TextMeshProUGUI heroBarrier;

    [SerializeField] private Image spellImage;
    [SerializeField] private Image spellITypeImage;
    [SerializeField] private List<Image> ranges = new List<Image>();

    [SerializeField] private TextMeshProUGUI spellName;
    [SerializeField] private TextMeshProUGUI spellExplain;
    [SerializeField] private TextMeshProUGUI spellCost;

    public int index { get; set; } = -1;

    private void Awake()
    {
        uiManager = GameManager.GetManagerClass<UIManager>();
        characterManager = GameManager.GetManagerClass<CharacterManager>();
    }

    public void DisplayCard(Sprite sprite, string name, string explain, int cost, int power, int barrier)
    {
        Spell.SetActive(false);
        Hero.SetActive(true);

        heroImage.sprite = sprite;
        heroName.text = name;
        heroExplain.text = explain;
        heroCost.text = string.Format("{0",cost);
        heroPower.text = string.Format("{0}", power);
        heroBarrier.text = string.Format("{0", barrier);
    }

    public void DisplayCard(Sprite spell, Sprite type, string name, string explain , int cost, ref int[] array)
    {
        Spell.SetActive(true);
        Hero.SetActive(false);

        spellImage.sprite = spell;
        spellITypeImage.sprite = type;

        spellName.text = name;
        spellExplain.text = explain;
        spellCost.text = string.Format("{0}", cost);
    }

    public void QuitButton()
    {
        gameObject.SetActive(false);
    }

    public void RemoveButton()
    {
        
        if (index == -1) return;
       
        characterManager.RemovePlayerCard(index);
        gameObject.SetActive(false);
    }



}
