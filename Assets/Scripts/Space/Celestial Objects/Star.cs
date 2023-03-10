using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StarColor
{
    Blue = 3,
    Yellow = 2,
    Red = 1
}

public interface SpaceObject
{
    void Initialize();
    void Activate();
    void Deactivate();
    Vector3 Position { get;  }
}

public class Star: MonoBehaviour, SpaceObject {
    public StarColor color;
    public float radius;
    public PlanetFactory planetFactory;

    private const float baseStarIntensity = 5000;

    public float Intensity { get { return (int)color * radius * baseStarIntensity; } } //Min intensity is radius/2 (Red dwarf). Max intensity is radius*6 (Blue giant). 

    [SerializeField]
    private float displayIntensity;

    public Material blue, yellow, red;
    public Color blueLight, yellowLight, redLight;
    private static StarColor[] colorDistribtion { get { return new StarColor[]{
        StarColor.Blue,
        StarColor.Yellow,
        StarColor.Yellow,
        StarColor.Red,
        StarColor.Red
    }; } }
    
    public Vector3 Position { get { return transform.position; } }
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

   public void Initialize()
    {
        MeshRenderer m = gameObject.GetComponent<MeshRenderer>();
        color = colorDistribtion[Random.Range(0, colorDistribtion.Length)];

        radius = radius*2;
        transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        
        switch (color)
        {
            case StarColor.Blue:
                m.material = blue;
                break;
            case StarColor.Yellow:
                m.material = yellow;
                break;
            case StarColor.Red:
                m.material = red;
                break;
        }

        displayIntensity = Intensity;

        if(GameData.Chance(1f))
        {
            SolarSystem s = gameObject.AddComponent<SolarSystem>();
            s.Initialize();
        }
    }

}
