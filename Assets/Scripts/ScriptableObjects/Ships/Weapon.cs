using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PrimaryWeapon,
    SecondaryWeapon,
    Ability
}




public class Weapon: MonoBehaviour
{
    //CanFire determines when the weapon can fire for AIs
    public virtual bool canFire { get; }
    public virtual bool needsReload { get; }

    public float range;
    public float cooldown;
    protected float cooldownTimer;

    public float fireTime;
    protected float fireTimer;

    public virtual void Awake()
    {

    }

    public virtual void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            cooldownTimer = 0;
        }

        if(fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
        }
        else
        {
            fireTimer = 0;
            cooldownTimer = cooldown;
        }

    }

    public virtual void Fire(GameObject activationSource)
    {

    }

    public virtual void StopFiring()
    {

    }
}


//[CreateAssetMenu(fileName = "Weapon", menuName = "Modules/ShipModules/Weapon")]


