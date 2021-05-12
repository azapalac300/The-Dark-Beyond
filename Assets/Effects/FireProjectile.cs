using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : WeaponEffect
{
    public GameObject projectile;


    public int damage;


    public override void Fire(GameObject activationSource)
    {
        Vector3 fwdVector = activationSource.transform.forward;

        fwdVector = fwdVector.normalized * range;
    }

}
