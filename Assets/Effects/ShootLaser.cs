using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : WeaponEffect
{
    public LineRenderer lineRenderer;
    public Color laserColor;


    public int damage;

    public override void Fire(GameObject activationSource)
    {
        Vector3 fwdVector = (activationSource.transform.forward).normalized*range + activationSource.transform.position;
        Vector3 start = activationSource.transform.position;

        
        //Render the laser
        Debug.DrawLine(start, fwdVector, Color.magenta);
        lineRenderer.SetPositions(new Vector3[] {start, fwdVector });
       
        //Do damage
        RaycastHit hit;
        Ray ray = new Ray(activationSource.transform.position, fwdVector.normalized);
        if(Physics.Raycast(ray, out hit, range))

        {
            if(hit.collider.tag == "Destructible")
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage, DamageType.Ballistic);
            }
        }
    }

}
