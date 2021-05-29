using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hull", menuName = "Modules/ShipModules/Hull")]
public class Hull: ShipModule
{
    public override ShipModuleType Type { get { return ShipModuleType.Hull; } }


    public float maxHP;

    private float currHP;

    public float CurrHP { get { return currHP; } }

    public void Awake()
    {
        currHP = maxHP;
    }
    public override bool TakeModuleDamage(float damageToTake, DamageType damageType)
    {

        if (currHP <= 0)
        {
            return false;
        }

        currHP -= damageToTake;
        return true;

    }
}
