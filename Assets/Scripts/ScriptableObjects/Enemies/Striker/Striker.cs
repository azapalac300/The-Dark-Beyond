using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Striker : EnemyBehavior
{ 

    public WeaponEffect weapon;

    public float turnSpeed;

    public override void Start()
    {
        base.Start();

        //GameObject weaponObj = Instantiate(weapon.weaponEffectObj, transform.position, transform.rotation);

        
    }


    public void Update()
    {
        HandleShooting();


    }


    public void HandleShooting()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist < weapon.range)
        {
            transform.LookAt(player.transform);
            weapon.Fire(gameObject);
            
        }
    }
}
