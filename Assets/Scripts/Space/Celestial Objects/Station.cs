using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Station : MonoBehaviour, Planet
{

    public Star parentStar { get; set; }
    public PlanetType Type { get; set; }
    public float orbitSpeed { get; set; }
    public float rotateSpeed { get; set; }
    public int orbitDir { get; set; }
    public float distanceFromStar { get; set; }
    public int rotateDir { get; set; }

    private SceneControl sceneControl;

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {

    }

    public void Land()
    {
        Debug.Log("Landing on space station");

        GameControl.LoadInterior();
    }
}
