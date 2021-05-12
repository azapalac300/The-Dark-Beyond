using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Striker : EnemyBehavior
{ 

    public Weapon weapon;

    public float turnSpeed;

    

    public void Update()
    {
        HandleShooting();


    }


    public void HandleShooting()
    {
        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist < weapon.range)
        {
            
            weapon.Fire(gameObject);
            transform.LookAt(player.transform);
        }
    }
}
