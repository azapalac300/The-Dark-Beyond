using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Striker : EnemyBehavior
{ 
    private enum ShootingState
    {
        idle, shooting, reloading
    }

    public Weapon weapon;


    private ShootingState strikerState;

    public GameObject model;

    public Explosion explosion;
    bool destroyed;
    public override void Start()
    {
        base.Start();


        explosion.explosionDone += () => { Destroy(gameObject); };
    }


    public override void Update()
    {


        if (destroyed) return;

        HandleFollowPlayer();
        HandleShooting();

        base.Update();
    }


    public void HandleFollowPlayer()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);
        if (dist < maxChaseDist && dist > minChaseDist)
        {

            Vector3 diff = (transform.position - player.transform.position).normalized;


            transform.Translate(diff * speed * Time.deltaTime);

            transform.LookAt(player.transform);




        }
    }

    public override void TakeDamage(float damageAmount, DamageType damageType)
    {

        //Do some other stuff
        base.TakeDamage(damageAmount, damageType);
    }


    public void HandleShooting()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        switch (strikerState)
        {
            case ShootingState.idle:
                if (dist <= weapon.range)
                {
                    strikerState = ShootingState.shooting;
                }
                weapon.StopFiring();

                break;
            case ShootingState.shooting:
                if (weapon.needsReload)
                {
                    strikerState = ShootingState.reloading;
                }

                if (dist > weapon.range)
                {
                    strikerState = ShootingState.idle;
                }


                weapon.Fire(gameObject);

                break;
            case ShootingState.reloading:

                if (weapon.canFire)
                {
                    strikerState = ShootingState.shooting;
                }

                weapon.StopFiring();
                break;
        }
    }

    public override void Destroyed()
    {
        if (!destroyed)
        {
            model.SetActive(false);
            explosion.PlayExplosion();
            DropLoot();
            destroyed = true;

        }
    }


}

