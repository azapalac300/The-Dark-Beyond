using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissiles : Weapon
{

    public GameObject missilePrefab;
    public float spawnRadius;

    public int missilesToSpawn;

    public bool homing;
    public float damage;

    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Fire(GameObject activationSource)
    {
        if (cooldownTimer <= 0)
        {
            target = activationSource.transform.forward * range + activationSource.transform.position;
            for (int i = 0; i < missilesToSpawn; i++)
            {
                Vector3 spawnPoint = Random.insideUnitSphere.normalized * spawnRadius + activationSource.transform.position;
                Missile missile = Instantiate(missilePrefab, spawnPoint, Quaternion.identity).GetComponent<Missile>();
                missile.Initialize(target, transform.parent.gameObject, DamageType.Ballistic, damage, homing);
            }

            cooldownTimer = cooldown;
        }
    }
}
