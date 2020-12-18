using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu][System.Serializable]
public class CrewData : ScriptableObject
{
    public Sprite icon;
    public string crewName;

    [Range(0, 150)][SerializeField]
    public float morale;

    public List<Ability> abilities;

    public float MoralePercent
    {
        get
        {
            return morale / 100;
        }
    }

    public int level {
        get
        {
           return Mathf.FloorToInt(Mathf.Sqrt(XP/6)) + 1;
        }
    }

    

    [Range(0, 10000)]
    public int XP;

}

