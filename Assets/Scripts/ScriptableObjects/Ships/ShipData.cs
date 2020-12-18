using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][System.Serializable]
public class ShipData : ScriptableObject
{
    public int HP;
    public int XP;
    public float maneuverability;
    public float moveSpeed;
    public string shipName;
    public int maxPrimaryWeapons;
    public int maxSecondaryWeapons;

    [Range(0, 3)]
    public int maxAbilities;

    public List<CrewData> crew;

    public List<ItemData> items;

}
