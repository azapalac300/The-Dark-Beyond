using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteriorBuilder : MonoBehaviour
{
    public int interiorMin;
    public int interiorMax;

    //Controls how "Branchy" or dense the station ends up feeling. 
    [Range (0, 1)]
    public float branchFactor;

    [SerializeField]
    public List<GameObject> floorPrefabs;


    [SerializeField]
    public List<GameObject> corridorPrefabs;

    
    //Start from the center out first, since that's easiest
    public Room roomSeed;
    public float roomOffset;
    public Room originRoom;
    private RandomQueue<Room> roomQueue;

    private List<Room> rooms;

    public GameObject refuel, bar, partShop;
    public GameObject characterPrefab;

    public Dictionary<string, Room> coordDict;

    public void Start()
    {
        roomQueue = new RandomQueue<Room>();
        rooms = new List<Room>();

      
        BuildInterior();
        FurnishInterior();
    }

    public void Update()
    {
        
    }

    public void BuildInterior()

    {
        coordDict = new Dictionary<string, Room>();
        int interiorSize = UnityEngine.Random.Range(interiorMin, interiorMax);
        //Place initial element into room random queue
        bool hasBar = false;
        bool hasPartShop = false;

        GameObject roomSeedObject = Instantiate(roomSeed.gameObject, transform.position, Quaternion.identity);
        roomSeedObject.transform.SetParent(transform);
        Room origin = roomSeedObject.GetComponent<Room>();
        originRoom = origin;
        origin.Initialize();
        string originCoords = origin.SetCoords(0, 0);
        coordDict.Add(originCoords, origin);
        CreateSpecialRoom(RoomFunction.Refuel, ref origin);
        roomQueue.Push(origin);
        rooms.Add(origin);

        for(int i = 0; i < interiorSize - 1;)
        {
            Room room = roomQueue.Pop();

            if(room.availableDirections.Count == 0)
            {
                roomQueue.RemoveLatestPop();
            }
            else
            {
                float factor = 1;
                for(int j = 0; j < 4; j++)
                {
                    if (room.availableDirections.Contains((Direction)j))
                    {
                        
                        if (UnityEngine.Random.Range(0f, 1f) <= factor && i < interiorSize - 1)
                        {
                            //Create a new room
                            i++;

                            Room newRoom = room.SpawnNeighboringRoom(roomSeed.gameObject, (Direction)j, roomOffset, transform, ref coordDict);

                            if (newRoom != null)
                            {
                                if (i >= interiorSize / 3 && hasPartShop == false)
                                {
                                    CreateSpecialRoom(RoomFunction.PartShop, ref newRoom);
                                    hasPartShop = true;
                                }

                                if (i >= interiorSize / 2 && hasBar == false)
                                {
                                    CreateSpecialRoom(RoomFunction.Bar, ref newRoom);
                                    hasBar = true;
                                }
                                rooms.Add(newRoom);
                                roomQueue.Push(newRoom);
                                factor *= branchFactor;
                            }


                        }
                    }
                }

            }

        }

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].FindRoomsNearby(coordDict);
            
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            rooms[i].DetermineRoomType();

        }

    }

    public void FurnishInterior()
    {
        for(int i = 0; i < rooms.Count; i++)
        {

            GameObject floorStyle = floorPrefabs[UnityEngine.Random.Range(0, floorPrefabs.Count)];
            GameObject corridorStyle = corridorPrefabs[UnityEngine.Random.Range(0, corridorPrefabs.Count)];
            rooms[i].FurnishRoom(floorStyle, corridorStyle);
        }
    }

    public void SpawnNPCs()
    {

    }

    public void SpawnCrew()
    {

    }

    public void CleanupInterior()
    {
        for(int i = 0; i < rooms.Count; i++)
        {
            Destroy(rooms[i].gameObject);

        }
        rooms.Clear();
        coordDict.Clear();
    }

    //Transforms a standard room into a special type of room
    public void CreateSpecialRoom(RoomFunction roomType, ref Room room)
    {
        if(room.roomFunction != RoomFunction.Standard)
        {
            return;
        }

        room.roomFunction = roomType;
        GameObject prefab = null;
        Vector3 spawnPosition = room.gameObject.transform.position + new Vector3(0, 5f, 0);
        switch (roomType)
        {
            case RoomFunction.Refuel:
                prefab = refuel;
                break;
            case RoomFunction.PartShop:
                prefab = partShop;
                break;
            case RoomFunction.Bar:
                prefab = bar;
                break;
        }



        if(prefab != null)
        {

            GameObject g = Instantiate(prefab, spawnPosition, Quaternion.identity);
            g.transform.SetParent(transform);
        }


    }
}
