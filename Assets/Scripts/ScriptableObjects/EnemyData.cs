using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public int HP;
    public float overworldSpeed;
    public float chaseRadius;
    public int bounty;
    public int lootAmount;
    public float lootSpawnRadius;
    public bool canDropLegendary;
    public bool canDropExotic;
    public float value;

    /*Some other stuff*/
}

