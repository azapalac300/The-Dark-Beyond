using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyBehavior: MonoBehaviour, Destructible {

    public EnemyData data;

    public Player player;

    public GameObject LootPackPrefab;

    public Image HP_bar;

    public Image UI_Indicator;

    protected float maxDisplayDist;
    protected float minDisplayDist;

    protected float speed;


    protected float turnSpeed;

    protected float maxChaseDist;

    protected float minChaseDist;



    protected float currHP;
    protected float maxHP;

    public virtual void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        currHP = data.HP;
        maxHP = data.HP;
        speed = data.speed;

        maxDisplayDist = data.maxDisplayDist;
        minDisplayDist = data.minDisplayDist;


        maxChaseDist = data.maxChaseRadius;
        minChaseDist = data.minChaseRadius;


        HP_bar.gameObject.SetActive(false);
        UI_Indicator.gameObject.SetActive(false);
    }

    public virtual void Update()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);


        if (dist < maxDisplayDist && dist > minDisplayDist)
        {
            UI_Indicator.gameObject.SetActive(true);
        }
        else
        {
            UI_Indicator.gameObject.SetActive(false);
        }
    }

    public virtual void TakeDamage(float damageAmount, DamageType damageType)
    {
        currHP -= damageAmount;

        HP_bar.gameObject.SetActive(true);

        HP_bar.fillAmount = currHP / maxHP;
        if(currHP <= 0)
        {
            Destroyed();
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
    }
}
