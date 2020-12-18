using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][System.Serializable]
public class ItemData : ScriptableObject
{
    public string itemName;

    public int amount;

    public Sprite icon;
}
