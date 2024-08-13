using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CardData : ScriptableObject
{
    
    public string CardName;
    public Sprite sprite;
    public enum CardType { Hero, Spell }
    public CardType cardType;
    public enum SpellType {Dest, Summon, Protect, Util}
    public SpellType spellType;

    public int CardCost;
    public int CardPower;
    public int CardBarrier;
    public int CardId;

    public string CardExplain;

    public int[] DamageArea=new int[9];

}
