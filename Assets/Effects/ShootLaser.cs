using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : Weapon
{
    public LineRenderer lineRenderer;
    public Color laserColor;
    private bool firing;
    public Material laserMaterial;
    public int damage;
    public DamageType damageType;

    public GameObject sparks;

    public float storedEnergy;
    public float currentStoredEnergy;
    public bool recovering;
    public float energyCost;
    public float recoveryRate;

    public override bool canFire { get { return currentStoredEnergy >= storedEnergy / 2; } }
    public override bool needsReload { get { return currentStoredEnergy <= 0; } }

    public override void Awake()
    {
        base.Awake();
        currentStoredEnergy = storedEnergy;
        lineRenderer.materials[0] = new Material(laserMaterial);
        laserMaterial = lineRenderer.materials[0];

        sparks.SetActive(false);
        laserMaterial.SetColor("MainColor", laserColor);
    }

    public override void StopFiring()
    {
         gameObject.SetActive(false);

        if (currentStoredEnergy < storedEnergy)
        {
            currentStoredEnergy += recoveryRate * Time.deltaTime;
        }
        else
        {
            currentStoredEnergy = storedEnergy;
        }
    }

    public override void Fire(GameObject activationSource)
    {
         if(currentStoredEnergy <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        currentStoredEnergy -= Time.deltaTime * energyCost;

        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        firing = true;
        Vector3 fwdVector = (activationSource.transform.forward).normalized*range + activationSource.transform.position;
        Vector3 start = activationSource.transform.position;
        Vector3 destination = fwdVector;
        
        
       
        //Do damage
        RaycastHit hit;
        Ray ray = new Ray(activationSource.transform.position, fwdVector.normalized);
        //Debug.DrawRay(activationSource.transform.position, fwdVector.normalized);
        
        if(Physics.Linecast(start, fwdVector, out hit))

        {

            if (hit.collider.tag == "Destructible")
            {
                
                hit.collider.gameObject.GetComponentInParent<Destructible>().TakeDamage(damage*Time.deltaTime, DamageType.Ballistic);
            }
            destination = hit.point;
            sparks.transform.position = hit.point;
            sparks.SetActive(true);
        }
        else
        {
            sparks.SetActive(false);
        }

        //Render the laser
        Debug.DrawLine(start, destination, Color.magenta);
        lineRenderer.SetPositions(new Vector3[] {start, destination });
    }

}
