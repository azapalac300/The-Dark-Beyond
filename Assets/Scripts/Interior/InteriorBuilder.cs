using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteriorBuilder : MonoBehaviour
{
    //Rotations
    private Quaternion A { get { return Quaternion.Euler(new Vector3(0, 0, 0)); } }
    private Quaternion B { get { return Quaternion.Euler(new Vector3(0, 90, 0)); } }
    private Quaternion C { get { return Quaternion.Euler(new Vector3(0, 180, 0)); } }
    private Quaternion D { get { return Quaternion.Euler(new Vector3(0, 270, 0)); } }

    private Quaternion RandomRotation()
    {
        Quaternion[] rotations = new Quaternion[]
        {
            A, B, C, D
        };

        return rotations[UnityEngine.Random.Range(0, rotations.Length)];
    }

    private enum ComponentType
    {
        U,
        I,
        L,
        T,
        X
    }

    public float roomOffset;
    public float hallOffset;

    //Rooms
    public GameObject[] rooms;

    //Sections
    public GameObject[] sections;

    [Space(10)]
    [SerializeField]
    private int numRooms;

    private GameObject GetRoom(ComponentType type)
    {
        return rooms[(int)type];
    }

    private GameObject GetSection(ComponentType type)
    {
        return sections[(int)type];
    }


    private ComponentType RandomComponent()
    {
        return (ComponentType)UnityEngine.Random.Range(0, 5);
    }

    RandomQueue<Vector3> nextLocations;
    public void Awake()
    {
        nextLocations = new RandomQueue<Vector3>();

        //When building the space station, start at 0
        nextLocations.Push(Vector3.zero);
    }


    public void BuildInterior(int numRooms)
    {
        this.numRooms = numRooms;
        BuildInterior();
    }
    public void BuildInterior()
    {
        //Build hangar
        GameObject hangar = Instantiate(GetRoom(ComponentType.U), nextLocations.Pop(), A);
        nextLocations.Push(new Vector3(0, 0, hallOffset));
        GameObject firstHall = Instantiate(GetSection(RandomComponent()), nextLocations.Pop(), RandomRotation());
        //Build common room


        //Build all other rooms
    }
}
