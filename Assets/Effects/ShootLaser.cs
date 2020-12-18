using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLaser : WeaponEffect
{
    public LineRenderer lineRenderer;
    public Color laserColor;


    public int damage;
    public float range;

    public override void Activate(GameObject activationSource)
    {
        Vector3 fwdVector = (activationSource.transform.forward).normalized*range;
        Vector3 start = activationSource.transform.position;
        //Render the laser
        Debug.DrawLine(start, fwdVector, Color.magenta, 5f);

        //Do damage
        RaycastHit hit;
        Ray ray = new Ray(activationSource.transform.position, fwdVector.normalized);
        if(Physics.Raycast(ray, out hit, range))

        {
            if(hit.collider.tag == "Destructible")
            {
                hit.collider.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

}
