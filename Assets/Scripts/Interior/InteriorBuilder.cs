using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteriorBuilder : MonoBehaviour
{
    public int interiorSize;

    //Controls how "Branchy" or dense the station ends up feeling. 
    [Range (0, 1)]
    public float branchFactor;
    
    //Start from the center out first, since that's easiest
    public Room roomSeed;
    public float roomOffset;
    private RandomQueue<Room> rooms;

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
        //Place initial element into room random queue

        GameObject roomSeedObject = Instantiate(roomSeed.gameObject, transform.position, Quaternion.identity);
        rooms.Push(roomSeedObject.GetComponent<Room>());

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
                            rooms.Push(newRoom);
                            factor *= branchFactor;
                        }
                    }
                }

            }

        }

    }
}
