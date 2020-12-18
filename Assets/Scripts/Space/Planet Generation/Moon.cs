using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Moons are a type of terrestrial planet!
//For now they'll be represented by a single sphere
public class Moon : MonoBehaviour, Planet
{
    public Star parentStar { get; set; }
    public PlanetType Type { get; set; }
    public float orbitSpeed { get; set; }
    public float rotateSpeed { get; set; }
    public int orbitDir { get; set; }
    public float distanceFromStar { get; set; }
    public int rotateDir { get; set; }


    public PlanetSkeleton skeleton;

    public void Initialize()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

    }

    

    public void Land()
    {

    }
}
