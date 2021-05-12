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

    [Space(10)]
    [SerializeField]
    EnemyBehavior enemyBehavior;


    /*Some other stuff*/
}

public class EnemyBehavior: ScriptableObject 
{
    public virtual void BehaviorUpdate(Player player, OverworldEnemy overworldEnemy)
    {

    }
}