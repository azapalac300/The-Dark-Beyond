using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Weapon
{
    public GameObject projectile;


    public int damage;

    public override void StopFiring()
    {
        base.StopFiring();
    }
    public override void Fire(GameObject activationSource)
    {
        Vector3 fwdVector = activationSource.transform.forward;

        fwdVector = fwdVector.normalized * range;
    }

}
