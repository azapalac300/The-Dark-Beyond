using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Ability
{
    public AbilityCategory category;
    public int index;
    public int level;
}

public enum AbilityCategory
{
    Engineering,
    Leadership,
    Piloting,
    LaserWeapons,
    BallisticWeapons,
    Diplomacy,
    Science
}


