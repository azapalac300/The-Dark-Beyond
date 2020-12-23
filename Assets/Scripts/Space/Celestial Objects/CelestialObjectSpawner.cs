using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObjectSpawner : MonoBehaviour {
    //Change this later to spawn entire solar systems. Just spawn individual planets for now.
    public int nSpaceObjects;
    private const int precision = 30;
    public float minRadius;
    private float maxRadius { get { return minRadius* 2; } }
    public SpaceObjectFactory spaceObjectfactory;
    public List<GameObject> spawnedObjects;
    public bool spawnSystems;
    public bool clearSystems;


    // Use this for initialization
    private void Awake()
    {
        spawnedObjects = new List<GameObject>();
        GameObject player = GameObject.Find("Player");

        //Have to set the player to inactive so its collider doesn't pollute the algorithm
        player.SetActive(false);
        GenerateSpaceObjects();
        player.SetActive(true);
    }

    void Update()
    {
        SpawnSystems();
        ClearSpawnedObjects();
    }

    void ClearSpawnedObjects()
    {
        if (clearSystems)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
            {
                DestroyImmediate(spawnedObjects[i]);
            }
            spawnedObjects.Clear();
            clearSystems = false;
        }
    }

    void SpawnSystems()
    {
        if (spawnSystems)
        {
            GenerateSpaceObjects();
            spawnSystems = false;
        }
    }

    void GenerateSpaceObjects()
    {

        RandomQueue<Vector3> randomQueue = new RandomQueue<Vector3>();
        Vector3 initialPoint = GenerateRandomPointAround(Vector3.zero);

        SpawnObject(initialPoint, ref randomQueue);

        for (int i = 0; i < nSpaceObjects - 1; i++)
        {
            Vector3 existingPoint = randomQueue.Pop();

            bool deleteFlag = false;
            for(int j = 0; j < precision; j++)
            {
                Vector3 candidate = GenerateRandomPointAround(existingPoint);

                Collider[] colliders = Physics.OverlapSphere(candidate, minRadius);

                if (colliders.Length == 0)
                {
                    SpawnObject(candidate, ref randomQueue);
                    deleteFlag = true;

                }
            }

            if (deleteFlag)
            {
                randomQueue.RemoveLatestPop();
            }

        }
    }

    Vector3 GenerateRandomPointAround(Vector3 center)
    {
        Vector2 v = (Random.insideUnitCircle.normalized * Random.Range(minRadius, maxRadius));
        return new Vector3(v.x, 0, v.y) + center;
    }


    void SpawnObject(Vector3 position,  ref RandomQueue<Vector3> randomQueue)
    {
        GameObject g = spaceObjectfactory.CreateSpaceObject(position, spawnedObjects.Count, SpaceObjectType.Star);
        randomQueue.Push(position);
        spawnedObjects.Add(g);
    }
}


public class RandomQueue<T>
{
    private List<T> items;

    public int randomIndex;
    public RandomQueue()
    {
        items = new List<T>();
        randomIndex = -1;
    }

    public int Count {  get { return items.Count; } }

    public void Push(T item)
    {
        items.Add(item);
    }

    public void RemoveLatestPop()
    {
        if(randomIndex > -1 && randomIndex < items.Count)
        {
            items.Remove(items[randomIndex]);
        }
    }
    
    public T Pop()
    {
     
        randomIndex = Random.Range(0, items.Count);
        T b = items[randomIndex];
        

        return items[randomIndex];
    }

    
}


