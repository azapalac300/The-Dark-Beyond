using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasGiant : MonoBehaviour, Planet
{

    public Star parentStar { get; set; }
    public PlanetType Type { get; set; }
    public float orbitSpeed { get; set; }
    public float rotateSpeed { get; set; }
    public int orbitDir { get; set; }
    public float distanceFromStar { get; set; }
    public int rotateDir { get; set; }

    public void Initialize()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        //Chance for moons to appear
        if (GameData.Chance(0.7f))
        {
            SolarSystem s = gameObject.AddComponent<SolarSystem>();
            s.GenerateMoons();
            s.Initialize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

    }
    public void Land()
    {
        Debug.Log("You cannot land on gas giants!");
    }
    
}
