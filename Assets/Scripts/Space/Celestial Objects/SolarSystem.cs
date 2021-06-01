using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Jobs;


public class SolarSystem : MonoBehaviour {
    public List<Planet> planets;
    public List<GameObject> planetObjects;
    public Star star;

    public int nPlanets;
    public PlanetFactory planetFactory;
    private bool generatingMoonsFromPlanet = false;

    public PlanetGenerator planetGenerator;


    public void Initialize()
    {
        planetFactory = Resources.Load<PlanetFactory>("PlanetFactory");
        planets = new List<Planet>();
        planetObjects = new List<GameObject>();

    }


    void Start () {
        planetGenerator = GameObject.Find("PlanetGenerator").GetComponent<PlanetGenerator>();

        if(planetGenerator == null)
        {
            Debug.LogError("ERROR: Planet Generator not found");
            return;
        }

        GeneratePlanets();

    }


    public void GenerateMoons()
    {
        generatingMoonsFromPlanet = true;
       
    }

    private void GeneratePlanets()
    {
        
        float[][] ranges;
        if (!generatingMoonsFromPlanet) {
            nPlanets = UnityEngine.Random.Range(1, GameData.planetRanges.GetLength(0) + 1);
            ranges = GameData.planetRanges;
        }
        else
        {
            nPlanets = UnityEngine.Random.Range(1, GameData.moonRanges.GetLength(0) + 1);
            ranges = GameData.moonRanges;
        }



        for (int i = 0; i < nPlanets; i++)
        {

            float dist = UnityEngine.Random.Range(ranges[i][0], ranges[i][1]);
            Vector3 planetSpawnPos = gameObject.transform.position;
            //We are on the x-z plane so either x or z will work
            planetSpawnPos.x += dist;

            //Creates a random planet type
            PlanetType typeToSpawn = (PlanetType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(PlanetType)).Length);
            //Only create gas giants and prototype moons for now
            GameObject planetObject = null;
            if (generatingMoonsFromPlanet)
            {
                if (GameData.Chance(0.5f))
                {
                    planetObject = planetFactory.Moon();
                }
                else
                {
                    planetObject = planetFactory.SpaceStation();
                }
            }
            else
            {
                if (GameData.Chance(0.0f))
                {
                    planetObject = planetFactory.GasGiant();
                }
                else
                {
                    planetObject = planetGenerator.GenerateTerrestrialPlanetObject();
                   
                }
            }

            Planet planet = planetObject.GetComponent<Planet>();

            if (generatingMoonsFromPlanet)
            {
                planet.parentStar = GetComponent<Planet>().parentStar;
            }
            else
            {
                planet.parentStar = GetComponent<Star>();
            }
            
          
            planet.orbitSpeed = UnityEngine.Random.Range(5f, 25f);
            planet.rotateSpeed = UnityEngine.Random.Range(10f, 20f);

            //Create a small chance for retrograde orbits
            if (GameData.Chance(0.2f))
            {
                planet.orbitDir = -1;
            }
            else
            {
                planet.orbitDir = 1;
            }


            planet.Initialize();
            planetObject.transform.parent = transform;
            planetObject.transform.position = planetSpawnPos;

            planet.distanceFromStar = Vector3.Distance(planetObject.transform.position, transform.position);

            planets.Add(planet);
            planetObjects.Add(planetObject);

        }
    }


    private void Update()
    {
        if(planetObjects == null)
        {
            Debug.Log("ERROR at: " + gameObject.name);
        }
        
        
        for(int i = 0; i < planetObjects.Count; i++)
        {
            
            Vector3 prevPos = planetObjects[i].transform.position;
            Vector3 currPos = planetObjects[i].transform.position;
            Vector3 diff = transform.position - currPos;
            diff.y = 0;

            Vector3 direction = new Vector3(-diff.z, 0, diff.x);
            //Debug.DrawLine(planetObjects[i].transform.position, planetObjects[i].transform.position + direction, Color.cyan);
            direction = direction.normalized;

            Vector3 movement = (direction * Time.deltaTime * planets[i].orbitSpeed * planets[i].orbitDir);
            
           

            //correct for distance
             Vector3 toStar = (transform.position - currPos);

            // Debug.DrawLine(planetObjects[i].transform.position, planetObjects[i].transform.position + toStar);

            toStar = toStar.normalized;
            float distanceDelta = Vector3.Distance(planetObjects[i].transform.position, transform.position) - planets[i].distanceFromStar;

            movement += toStar * distanceDelta;

            planetObjects[i].transform.position += movement;

            //Debug.DrawLine(prevPos, planetObjects[i].transform.position, Color.magenta, 600f);
        }
        

    }


}


