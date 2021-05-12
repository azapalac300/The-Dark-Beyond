using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShipUI : MonoBehaviour
{
    public Ship ship;

    public Image hpMeter;
    public Image shieldMeter;
    public Image jumpFuelMeter;



    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        CheckModules();

        hpMeter.fillAmount = ship.HP_ratio();
        shieldMeter.fillAmount = ship.Shields_ratio();
        jumpFuelMeter.fillAmount = ship.JumpFuel_ratio();
        
    }

    public void CheckModules() 
    {
        if (ship.HasModule<JumpDrive>())
        {
            jumpFuelMeter.gameObject.SetActive(true);
            
        }
        else
        {
            jumpFuelMeter.gameObject.SetActive(false);

        }

        if (ship.HasModule<Shields>())
        {
            shieldMeter.gameObject.SetActive(true);
        }
        else
        {
            shieldMeter.gameObject.SetActive(false);
        }

        if (ship.HasModule<Hull>())
        {
            hpMeter.gameObject.SetActive(true);
        }
        else
        {
            hpMeter.gameObject.SetActive(true);
        }

    }


   
}
