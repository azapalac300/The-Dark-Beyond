using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shields : ShipModule
{
    public override ShipModuleType Type { get { return ShipModuleType.Shields; } }

    public float maxShields;

    [SerializeField]
    private float currShields;

    public override bool TakeModuleDamage(float damageToTake, DamageType damageType)
    {

        if(currShields <= 0)
        {
            return false;
        }

        currShields -= damageToTake;
        return true;
        
    }

    public float CurrShields { get { return currShields; } }
}
