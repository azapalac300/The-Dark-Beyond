using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum ShipModuleType
{
    Base,
    JumpDrive,
    Engine,
    Hull,
    Shields,
    Bridge,
    PrimaryWeapon,
    SecondaryWeapon,
    Ability0,
    Ability1,
    Ability2

}

public class ShipModule :ScriptableObject
{
    public virtual ShipModuleType Type { get { return ShipModuleType.Base; } }

    public virtual void UpdateModule()
    {

    }
}
