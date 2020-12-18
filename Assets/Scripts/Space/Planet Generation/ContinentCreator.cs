using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinentCreator {

    public List<Vertex> continentCenters;

    public HashSet<int> terraformedIndices;

    public float terrainDelta;

    public List<Continent> continents;

    public TerrestrialPlanetType Type;

    public Vector3 planetPos;

    public int nContinents;

    public MinMax minmax;

   

    public static int maxElevationFactor { get { return 1; } }
    //Remember this is a rough estimate
    public int continentSize;


    public static float scaleFactor { get { return 1 / 1.5f; } }

    public ContinentCreator()
    {
        continentCenters = new List<Vertex>();
        terraformedIndices = new HashSet<int>();
        minmax = new MinMax();
        //Test the algorithm with 1 continent. Need to restructure the code to do multiple
        nContinents = 13;
        continents = new List<Continent>();
    }


    public void SeedContinents(ref PlanetSkeleton p, ref MinMax minmax)
    {
        //Create random list
        List<int> randomSelection = new List<int>();
        List<int> randomIndices = new List<int>();
        for (int i = 0; i < p.vertices.Count; i++)
        {

            randomSelection.Add(i);
        }



        continentSize = Mathf.RoundToInt(p.vertices.Count / nContinents);
        terrainDelta = p.radius/10;
        planetPos = p.position;

        for (int i = 0; i < nContinents; i++)
        {
            //Only create continents if there are points left to create continents with
            if (randomSelection.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, randomSelection.Count);
                int randomPoint = 0;
                try
                {
                    randomPoint = randomSelection[randomIndex];
                }
                catch (System.Exception e)
                {
                    Debug.Log(e.Message);
                }

                continentCenters.Add(p.vertices[randomPoint]);


                Continent newContinent = CreateContinent(p.vertices[randomPoint], ref randomSelection);

                //For each continent, raise or lower it
                float[] elevations = {0, 0, 0, 1};
                float elevation = elevations[UnityEngine.Random.Range(0, elevations.Length)];


                SetElevation(newContinent, elevation, scaleFactor);

                if (newContinent.type == ContinentType.Land)
                {

                    CreateMountains(newContinent, scaleFactor);
                }
                

                continents.Add(newContinent);
            }
        }

        minmax = this.minmax;

    }



    public Continent CreateContinent (Vertex seed, ref List<int> selection)
    {
        Continent continent = new Continent();
        continent.vertices.Add(seed);
        terraformedIndices.Add(seed.index);

        RandomQueue<Vertex> randomQueue = new RandomQueue<Vertex>();
        HashSet<int> visitedIndices = new HashSet<int>();
        randomQueue.Push(seed);
        visitedIndices.Add(seed.index);
        selection.Remove(seed.index);
        //Continents can only be as big as the maximum continent size.
        //Any left over vertices will have height values set to the average height of their neighbors
        //Vertex count set to 1 because there is already a vertex in the continent
        //Adding a loop missed counter in case the continent ends up being small
        //If the random queue count is 0 it means we have run out of space 
        //since it will only remove "exhausted" nodes

        while (continent.vertices.Count < continentSize && randomQueue.Count > 0)
        {
            Vertex v = randomQueue.Pop();

            bool deleteFlag = true;

             for(int i = 0; i < v.neighbors.Count; i++)
            {
                int index = v.neighbors[i].index;

                if (!visitedIndices.Contains(index) && !terraformedIndices.Contains(index))
                {
                    deleteFlag = false; //There is still a node left to be explored

                    visitedIndices.Add(index);
                    terraformedIndices.Add(index);
                    selection.Remove(index);
                    
                    continent.vertices.Add(v.neighbors[i]);
                    randomQueue.Push(v.neighbors[i]);
                }

            }

            if (deleteFlag)
            {
                randomQueue.RemoveLatestPop();
            }
        }


        return continent;
    }

    //Sets the elevation for the entire continent
    public void SetElevation(Continent continent, float elevation, float adjustFactor)
    {
        if(adjustFactor == 0)
        {
            continent.type = ContinentType.Ocean;

        }
        else
        {
            continent.type = ContinentType.Land;

        }

        for (int i = 0; i < continent.vertices.Count; i++)
        {
            continent.vertices[i].elevation = elevation;

            if (continent.type == ContinentType.Land)
            {
                AdjustVertex(continent.vertices[i], elevation * adjustFactor);
            }
        }
    }

    public void CreateMountains(Continent continent, float adjustFactor)
    {
        int nMountains = UnityEngine.Random.Range(0, Mathf.FloorToInt(continent.vertices.Count/3));
        List<int> mountainCandidates = new List<int>();
        for(int i = 0; i < continent.vertices.Count; i++)
        {
            mountainCandidates.Add(i);
        }

        for(int i = 0; i < nMountains; i++)
        {
            int r = UnityEngine.Random.Range(0, mountainCandidates.Count);

            Vertex v = continent.vertices[mountainCandidates[r]];
            v.elevation = maxElevationFactor;
            AdjustVertex(v, adjustFactor);


           mountainCandidates.Remove(mountainCandidates[r]);
        }
    }


    public void AdjustVertex(Vertex vertex, float adjustFactor)
    {
        Vector3 position = vertex.position;

        position -= planetPos;
        position.Normalize();

       
        vertex.position += position * terrainDelta * adjustFactor;
        vertex.cell.heightVal += terrainDelta * adjustFactor;

        minmax.AddValue(Vector3.Distance(position, vertex.position));
    }

}

public enum ContinentType
{
    Land,
    Ocean
}

public class Continent
{
    public List<Vertex> vertices;
    public int heightVal;
    public ContinentType type;
    public Continent()
    {
        vertices = new List<Vertex>();
    }

   

}