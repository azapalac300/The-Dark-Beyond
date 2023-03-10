using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public interface Planet {
    void Initialize();
    Star parentStar { get; set; }
    PlanetType Type { get; set; }
    float rotateSpeed { get; set; }
    float orbitSpeed { get; set; }
    int orbitDir { get; set; }
    int rotateDir { get; set; }
    void Land();



    float distanceFromStar { get; set; }
}

public enum TerrestrialPlanetType
{
    Random, //Placeholder value

    Ice,
    Terran,
    Desert,
    Exotic,
    Storm,
    Lava,
    Lunar,
}

public enum TerrestrialPlanetTypePride
{
    //Pride Planets
    /*
    Gay,
    Lesbian,
    Bi,
    Trans,
    Ace,*/
}


[ExecuteInEditMode]
public class TerrestrialPlanet : MonoBehaviour, Planet {
    public float radius;
    public PlanetSkeleton skeleton;
    public GameObject oceanPrefab;
    public BiomeColorSettings biomeColorSettings;


    TerrainGenerator terrainGenerator;
    MeshRenderer planetMeshRenderer;
    MeshFilter planetMeshFilter;
    ColorGenerator colorGenerator = new ColorGenerator();
    ContinentCreator creator = new ContinentCreator();

    public float rotateSpeed { get; set; }
    public Star parentStar { get; set; }
    public PlanetType Type { get; set; }
    public float orbitSpeed { get; set; }
    public int orbitDir { get; set; }
    public float distanceFromStar { get; set; }
    public int rotateDir { get; set; }

    private string settingsPath;


    [SerializeField]
    public PlanetType type; //For display purposes only
    public TerrestrialPlanetType terrestrialType;

    public MinMax terrainMinMax;

    public float squareDistance;

	// Use this for initialization
	public void Initialize () {
        type = PlanetType.Terrestrial;
        Type = type;

        if (terrestrialType == TerrestrialPlanetType.Random)
        {
            terrestrialType = (TerrestrialPlanetType)(UnityEngine.Random.Range(1, Enum.GetNames(typeof(TerrestrialPlanetType)).Length));
        }


        UpdateBiomeSettings();
        terrainMinMax = new MinMax();
        terrainMinMax.AddValue(skeleton.radius + 5);
        terrainMinMax.AddValue(skeleton.radius - 5);
        creator.SeedContinents(ref skeleton, ref terrainMinMax);

        PlanetMeshGenerator.GenerateMesh(skeleton);

        planetMeshRenderer = GetComponent<MeshRenderer>();
        planetMeshFilter = GetComponent<MeshFilter>();

        colorGenerator.UpdateColorSettings(ref planetMeshRenderer, biomeColorSettings);

        colorGenerator.UpdateElevation(terrainMinMax);
        colorGenerator.UpdateColors();

        InitializeLighting();
        
        //TODO: Need to determine whether an ocean can even spawn on this planet
        rotateSpeed = UnityEngine.Random.Range(-10f, 10f);
       
    }

    public void InitializeLighting()
    {
        //Set light color
        StarlightColors starlightColors = (StarlightColors)Resources.Load("StarlightColors");
        switch (parentStar.color)
        {
            case StarColor.Blue:
                planetMeshRenderer.material.SetColor("_StarLightColor", starlightColors.blueStarColor);
                break;

            case StarColor.Red:
                planetMeshRenderer.material.SetColor("_StarLightColor", starlightColors.redStarColor);
                break;

            case StarColor.Yellow:
                planetMeshRenderer.material.SetColor("_StarLightColor", starlightColors.yellowStarColor);
                break;
        }

        //Set light intensity
        squareDistance = distanceFromStar;
        float inverseSquare = parentStar.Intensity / (distanceFromStar * distanceFromStar);
        planetMeshRenderer.material.SetFloat("_StarLightIntensity", inverseSquare);
    }

    public void UpdateBiomeSettings()
    {
        string path = "Biomes/Standard/" + terrestrialType.ToString();

        if (path != settingsPath || biomeColorSettings == null)
        {
            biomeColorSettings = (BiomeColorSettings)Resources.Load(path);
            settingsPath = path;
        }
  
    }

    // Update is called once per frame
    void Update() {
        
        //if (EditorApplication.isPlaying || EditorApplication.isPaused) return;
        if (planetMeshRenderer == null) planetMeshRenderer = GetComponent<MeshRenderer>();


        UpdatePlanetLighting(ref planetMeshRenderer);

       // transform.Rotate(Time.deltaTime*Vector3.up*rotateSpeed);
    }

    void UpdatePlanetLighting(ref MeshRenderer planetMeshRenderer)
    {
        //Update the direction of the light to always point at the star
        Vector3 diff = parentStar.transform.position - transform.position;
        diff = diff.normalized;
        planetMeshRenderer.material.SetVector("_StarLightDirection", diff);


        Debug.DrawRay(transform.position, diff*1000f, Color.magenta);

    }


    public void Land()
    {
        TerrainData data = Resources.Load<TerrainData>("Terrain Data");
        data.skeleton = skeleton;
        SceneManager.LoadScene("Ground");
    }

}

