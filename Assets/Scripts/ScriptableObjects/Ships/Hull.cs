using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hull", menuName = "Modules/ShipModules/Hull")]
public class Hull : ShipModule
{
    public override ShipModuleType Type { get { return ShipModuleType.Hull; } }


    public int maxHP;

    [SerializeField]
    private int currHP;

    public int CurrHP { get { return currHP; } }
}
