using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Exotic,
    Legendary
}

[CreateAssetMenu][System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;

    public int amount;

    public Sprite icon;

    public Rarity rarity;

    public static Rarity RollRarity(float enemyValue = 0, bool canDropExotic = false, bool canDropLegendary = false)
    {
        float rarityValue = UnityEngine.Random.Range(0f, 1f);

        if (enemyValue < 0 || enemyValue > 10)
        {
            throw new ArgumentOutOfRangeException("Enemy Value", "Enemy value must be between 0 and 10");
            return Rarity.Common;
        }

        if ((rarityValue < .01f + enemyValue / 10000) && canDropLegendary)
        {
            return Rarity.Exotic;
        }

        if ((rarityValue < .015f + enemyValue / 1000) && canDropExotic)
        {
            return Rarity.Exotic;
        }

        if (rarityValue < .05f + enemyValue / 1000)
        {
            return Rarity.Epic;
        }

        if (rarityValue < .18f + enemyValue / 100)
        {
            return Rarity.Rare;
        }

        if (rarityValue < .5f + enemyValue / 50)
        {
            return Rarity.Uncommon;
        }

        return Rarity.Common;
    }

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

            case Rarity.Exotic:
                hexCode = "#E72DF3";
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
