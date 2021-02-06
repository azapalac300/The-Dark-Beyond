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
    
    //Start from the center out first, since that's easiest
    public Room roomSeed;
    public float roomOffset;
    private RandomQueue<Room> rooms;

    public GameObject refuel, bar, partShop;


    public void Start()
    {
        rooms = new RandomQueue<Room>();

        BuildInterior();
    }

    public void Update()
    {
        
    }

    public void BuildInterior()

    {
        int interiorSize = UnityEngine.Random.Range(interiorMin, interiorMax);
        //Place initial element into room random queue
        bool hasBar = false;
        bool hasPartShop = false;

        GameObject roomSeedObject = Instantiate(roomSeed.gameObject, transform.position, Quaternion.identity);

        Room origin = roomSeedObject.GetComponent<Room>();
        CreateSpecialRoom(RoomType.Refuel, ref origin);
        rooms.Push(origin);

        for(int i = 0; i < interiorSize - 1;)
        {
            Room room = rooms.Pop();

            if(room.availableDirections.Count == 0)
            {
                rooms.RemoveLatestPop();
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
                            Room newRoom = room.SpawnNeighboringRoom(roomSeedObject, (Direction)j, roomOffset);

                            if(i >= interiorSize / 3 && hasPartShop == false )
                            {
                                CreateSpecialRoom(RoomType.PartShop, ref newRoom);
                                hasPartShop = true;
                            }

                            if(i >= interiorSize/2 && hasBar == false)
                            {
                                CreateSpecialRoom(RoomType.Bar, ref newRoom);
                                hasBar = true;
                            }

                            rooms.Push(newRoom);
                            factor *= branchFactor;


                        }
                    }
                }

            }

        }

    }

    //Transforms a standard room into a special type of room
    public void CreateSpecialRoom(RoomType roomType, ref Room room)
    {
        room.roomType = roomType;
        GameObject prefab = null;
        Vector3 spawnPosition = room.gameObject.transform.position + new Vector3(0, 5f, 0);
        switch (roomType)
        {
            case RoomType.Refuel:
                prefab = refuel;
                break;
            case RoomType.PartShop:
                prefab = partShop;
                break;
            case RoomType.Bar:
                prefab = bar;
                break;
        }



        if(prefab != null)
        {

            GameObject g = Instantiate(prefab, spawnPosition, Quaternion.identity);
        }


    }
}
