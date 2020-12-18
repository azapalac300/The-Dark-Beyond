using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PrimaryWeapon = ShipModuleType.PrimaryWeapon,
    SecondaryWeapon = ShipModuleType.SecondaryWeapon,
    Ability0 = ShipModuleType.Ability0,
    Ability1 = ShipModuleType.Ability1,
    Ability2 = ShipModuleType.Ability2
}




public class WeaponEffect: MonoBehaviour
{
    public virtual void Activate(GameObject activationSource)
    {

    }
}


[CreateAssetMenu(fileName = "Weapon", menuName = "Modules/ShipModules/Weapon")]
public class Weapon : ShipModule
{
    public override ShipModuleType Type { get { return (ShipModuleType)weaponType; } }

    
    public WeaponType weaponType;

    [SerializeField]
    public WeaponEffect weaponEffect;
        
        // Start is called before the first frame update

    public float cooldown = 0;
    public float cooldownTimer;
   

    // Update is called once per frame
    public override void UpdateModule()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

    }

    public void Fire(GameObject activationSource)
    {
        if (cooldownTimer <= 0)
        {
            cooldownTimer = cooldown;
            weaponEffect.Activate(activationSource);
        }
    }


}


