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
        
        //TODO: Need to determine whether an ocean can even spawn on this planet
        rotateSpeed = UnityEngine.Random.Range(0f, 10f);

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
        
        if (EditorApplication.isPlaying || EditorApplication.isPaused) return;
        if (planetMeshRenderer == null) planetMeshRenderer = GetComponent<MeshRenderer>();

    }

    public void Land()
    {
        TerrainData data = Resources.Load<TerrainData>("Terrain Data");
        data.skeleton = skeleton;
        SceneManager.LoadScene("Ground");
    }

}

