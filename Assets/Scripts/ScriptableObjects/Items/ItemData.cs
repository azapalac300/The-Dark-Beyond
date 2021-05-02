using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic, 
    Legendary
}

[CreateAssetMenu][System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;

    public int amount;

    public Sprite icon;

    public Rarity rarity;

    public static Color GetRarityColor(Rarity rarity)
    {
        string hexCode = "#000000";
        switch (rarity)
        {
            case Rarity.Common:
                hexCode = "#999B9C";
                break;

            case Rarity.Uncommon:
                hexCode = "#009C11";
                break;

            case Rarity.Rare:
                hexCode = "#008CFF";
                break;


            case Rarity.Epic:
                hexCode = "#8F2DF3";
                break;


            case Rarity.Legendary:
                hexCode = "#F3502D";
                break;


        }

        Color color;

     

        ColorUtility.TryParseHtmlString(hexCode, out color);
        return color;
    }
}
