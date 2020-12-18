using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Docking : MonoBehaviour
{
    public bool canDock;
    public float dockRadius;
    private bool locked;
    public Planet parentPlanet { get; private set; }
    public GameObject dockIndicator;
    
    // Start is called before the first frame update
    void Start()
    {
        parentPlanet = GetComponent<Planet>();
    }

    // Update is called once per frame
    void Update()
    {

        if(dist() < dockRadius * 5)
        {
            dockIndicator.SetActive(true);
        }
        else
        {
            dockIndicator.SetActive(false);
        }

        if (!Player.instance.Docked)
        {
           
            canDock = dist() < dockRadius;

            if (locked)
            {
                locked = dist() < dockRadius * 2;
            }

            if (canDock && !locked)
            {
                Player.instance.Dock(this);
                locked = true;
            }

        }
        
    }
    private float dist()
    {

        return (Vector3.Distance(transform.position, Player.instance.Position));

    }

    
}
