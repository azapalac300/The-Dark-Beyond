using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shields", menuName = "Modules/ShipModules/Shields")]
public class Shields : ShipModule
{
    public override ShipModuleType Type { get { return ShipModuleType.Shields; } }

    public int maxShields;

    [SerializeField]
    private int currShields;

    public int CurrShields { get { return currShields; } }
}
