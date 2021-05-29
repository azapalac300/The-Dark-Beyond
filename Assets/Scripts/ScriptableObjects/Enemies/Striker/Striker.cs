using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Striker : EnemyBehavior
{ 
    private enum StrikerState
    {
        idle, shooting, reloading
    }
    public Weapon weapon;

    public float turnSpeed;

    private StrikerState strikerState;
    public override void Start()
    {
        base.Start();

        //GameObject weaponObj = Instantiate(weapon.weaponEffectObj, transform.position, transform.rotation);

        
    }


    public void Update()
    {
        HandleShooting();


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
            case StrikerState.idle:
                if (dist <= weapon.range)
                {
                    strikerState = StrikerState.shooting;
                }
                weapon.StopFiring();

             break;
            case StrikerState.shooting:
                if (weapon.needsReload)
                {
                    strikerState = StrikerState.reloading;
                }

                if (dist > weapon.range)
                {
                    strikerState = StrikerState.idle;
                }
                transform.LookAt(player.transform);

                weapon.Fire(gameObject);

                break;
            case StrikerState.reloading:

                if (weapon.canFire)
                {
                    strikerState = StrikerState.shooting;
                }

                weapon.StopFiring();
                break;
        }


    }
}
