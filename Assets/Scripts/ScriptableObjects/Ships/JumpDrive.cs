using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpDrive : ShipModule
{
    public override ShipModuleType Type { get { return ShipModuleType.JumpDrive; } }


    public float maxDistance;
    public float cooldownTime;
    public float fuelCapacity;
    public float fuelCost;


    public float CooldownTimeRemaining {  get { return cooldownTimer;  } }
    private float cooldownTimer;

    public float CurrFuel { get { return currentFuel; } }
    [SerializeField]
    private float currentFuel;

    public void JumpTo(Vector3 destination, Transform transform)
    {
        if (currentFuel >= fuelCost)
        {
            transform.position = destination;
            currentFuel -= fuelCost;

            cooldownTimer = 0;
        }
        
    }

    public override void UpdateModule()
    {
       if(cooldownTimer < cooldownTime)
        {
            cooldownTimer += Time.deltaTime;
        }

       if(cooldownTimer > cooldownTime)
        {
            cooldownTimer = cooldownTime;
        }
    }

}
