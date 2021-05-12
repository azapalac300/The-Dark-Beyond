using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    PrimaryWeapon,
    SecondaryWeapon,
    Ability
}




public class WeaponEffect: MonoBehaviour
{
    public virtual void Activate(GameObject activationSource)
    {

    }
}


//[CreateAssetMenu(fileName = "Weapon", menuName = "Modules/ShipModules/Weapon")]
public class Weapon: ScriptableObject
{
    
    public WeaponType weaponType;

    [SerializeField]
    public GameObject weaponEffectObj;
        
        // Start is called before the first frame update

    public float cooldown;
    public float cooldownTimer;


    public float range;

    // Update is called once per frame
    public virtual void UpdateWeapon()
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
            Vector3 fwdVector = (activationSource.transform.forward).normalized * range;
            Debug.DrawLine(activationSource.transform.position, activationSource.transform.position + fwdVector);
            cooldownTimer = cooldown;
            weaponEffectObj.GetComponent<WeaponEffect>().Activate(activationSource);
        }
    }


}


