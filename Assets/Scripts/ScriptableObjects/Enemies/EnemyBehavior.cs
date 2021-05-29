using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyBehavior: MonoBehaviour, Destructible {

    public EnemyData data;

    public Player player;

    public GameObject LootPackPrefab;

    public float currHP;

    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        currHP = data.HP;

    }

    public virtual void TakeDamage(float damageAmount, DamageType damageType)
    {
        currHP -= damageAmount;

        if(currHP <= 0)
        {
            Destroyed();
            DropLoot();
        }
    }

    public virtual void Destroyed()
    {
        //Base class does nothing
    }

    public void DropLoot()
    {

        for (int i = 0; i < data.lootAmount; i++)
        {

            Vector3 spawnPoint = (UnityEngine.Random.insideUnitSphere * data.lootSpawnRadius) + transform.position;
            LootPack lootPack = Instantiate(LootPackPrefab, spawnPoint, Quaternion.identity).GetComponent<LootPack>();
            Rarity rarity = ItemData.RollRarity(data.value, data.canDropExotic, data.canDropLegendary);

            lootPack.Initialize(rarity);
        }

        player.AddCredits(data.bounty);
        Destroy(gameObject);
    }
}
