using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Explosion : MonoBehaviour
{

    public float explosionTime;

    private float explosionTimer;

    public event Action explosionDone;

    private bool exploded;
    private bool messageSent;

    public List<ParticleSystem> particleSystems;
    // Start is called before the first frame update
    void Start()
    {
        messageSent = false;
        exploded = false;
        particleSystems = new List<ParticleSystem>();
        particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());

        Debug.Log("Particle Systems Count: " + particleSystems.Count);
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded && !messageSent)
        {
            if(explosionTimer > 0)
            {
                explosionTimer -= Time.deltaTime;
            }
            else
            {
                explosionDone?.Invoke();
                messageSent = true;
            }
        }
    }

    public void PlayExplosion()
    {
        for(int i = 0; i < particleSystems.Count; i++)
        {
            particleSystems[i].Play();
        }

        explosionTimer = explosionTime;
        exploded = true;
    }
}
