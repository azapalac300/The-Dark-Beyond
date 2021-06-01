using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update

  //  public Transform _target;

    public DamageType _damageType;

    public float _damageAmount;

    public float speed;

    public float explosionRadius;

    private Vector3 fwdVector;

    public float lifeTime;
    private float lifeTimer;

    public Explosion explosion;
    private bool exploded;

    public bool _homing;

    public GameObject model;

    public GameObject _activator;
     
    public void Initialize(Vector3 target, GameObject activator, DamageType damageType, float damageAmount, bool homing)
    {

        lifeTimer = 0;
        fwdVector = (target - transform.position).normalized;
        model.transform.rotation =  Quaternion.LookRotation(fwdVector)* Quaternion.Euler(90, 0, 0);


        _damageAmount = damageAmount;
        _damageType = damageType;
        _homing = homing;
        _activator = activator;

        explosion.explosionDone += () => {
            Destroy(gameObject); 
        };

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Explode();

        if (!exploded)
        {
            if (_homing)
            {
                HandleHoming();
            }


            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Destructible")
                {
                    if(colliders[i].transform.parent.name != _activator.name)
                    {
                        Explode();
                        break;
                    }

                    
                }


            }

            transform.Translate(fwdVector * speed * Time.deltaTime);
            lifeTimer += Time.deltaTime;
            bool flag = false;

            if ((lifeTimer >= lifeTime || flag))
            {
                Explode();
            }
        }
    }

    public void Explode()
    {

        model.SetActive(false);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.tag == "Destructible")
            {

                colliders[i].GetComponentInParent<Destructible>().TakeDamage(_damageAmount, _damageType);


            }


        }

        explosion.PlayExplosion();

        exploded = true;
    }
    public void HandleHoming()
    {

    }
}
