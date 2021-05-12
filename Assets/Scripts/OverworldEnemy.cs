using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class OverworldEnemy : MonoBehaviour
{

    public GameObject LootPackPrefab;


    private Player player;

    public EnemyData data;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

    }


    // Update is called once per frame
    void Update()
    {
       // HandleChase();
    }

  

    public void DropLoot()
    {

        for (int i = 0; i < data.lootAmount; i++) {

            Vector3 spawnPoint = (UnityEngine.Random.insideUnitSphere * data.lootSpawnRadius) + transform.position;
            LootPack lootPack = Instantiate(LootPackPrefab, spawnPoint, Quaternion.identity).GetComponent<LootPack>();
            Rarity rarity = ItemData.RollRarity(data.value, data.canDropExotic, data.canDropLegendary);

            lootPack.Initialize(rarity);
        }

        player.AddCredits(data.bounty);
        Destroy(gameObject);
    }
}
