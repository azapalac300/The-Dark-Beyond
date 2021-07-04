using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public int HP;
    public float speed;
    public float turnSpeed;


    public float maxChaseRadius;
    public float minChaseRadius;

    public int bounty;
    public int lootAmount;
    public float lootSpawnRadius;
    public bool canDropLegendary;
    public bool canDropExotic;
    public float value;

    public float maxDisplayDist;
    public float minDisplayDist;

    /*Some other stuff*/
}

