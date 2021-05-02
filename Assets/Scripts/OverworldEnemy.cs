using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OverworldEnemy : MonoBehaviour
{
    public Player player;

    public float chaseRadius;

    public float engageRadius;

    public float moveSpeed;


    Vector3 origPosition;

    public EnemyData data;

    public static Action<OverworldEnemy> SetupCombat;

    private float DistToPlayer {  get { return Vector3.Distance(transform.position, player.transform.position); } }

    private float DistToOrig { get { return Vector3.Distance(transform.position, origPosition); } }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();


        origPosition = transform.position;

        moveSpeed = data.overworldSpeed;
        chaseRadius = data.chaseRadius;

    }


    // Update is called once per frame
    void Update()
    {
        HandleChase();
    }

    public void HandleChase()
    {

        if ((DistToPlayer > chaseRadius || DistToOrig >= chaseRadius * 2) && DistToOrig > engageRadius)
        {


            Vector3 delta = (origPosition - transform.position);
             delta = delta.normalized;

            transform.Translate(delta * moveSpeed * Time.deltaTime);    

            return;
        }

        if (DistToPlayer <= chaseRadius)
        {
            //move towards player
            Vector3 delta = (player.transform.position - transform.position).normalized;

            transform.Translate(delta * moveSpeed * Time.deltaTime);

        }


        if (DistToPlayer <= engageRadius)
        {
            TurnBasedCombat.SetupCombat(this);
            GameControl.LoadCombat();
           
        }

    }

    public void DropLoot()
    {
        //Do some other stuff
        player.AddCredits(data.bounty);
        Destroy(gameObject);
    }
}
