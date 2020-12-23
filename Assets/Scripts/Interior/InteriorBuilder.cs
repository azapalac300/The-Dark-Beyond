using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InteriorBuilder : MonoBehaviour
{
    //Controls how "Branchy" or dense the station ends up feeling. 
    [Range (0, 1)]
    public float branchFactor;

    //Rotations
    public static Quaternion A { get { return Quaternion.Euler(new Vector3(0, 0, 0)); } }
    public static Quaternion B { get { return Quaternion.Euler(new Vector3(0, 90, 0)); } }
    public static Quaternion C { get { return Quaternion.Euler(new Vector3(0, 180, 0)); } }
    public static Quaternion D { get { return Quaternion.Euler(new Vector3(0, 270, 0)); } }


    private Dictionary<RoomRotation, Quaternion> rotations;
    private Quaternion[] rotationList;

    private Quaternion RandomRotation()
    {
       

        return rotationList[UnityEngine.Random.Range(0, rotationList.Length)];
    }

    private enum ComponentType
    {
        U,
        I,
        L,
        T,
        X
    }

    public GameObject seedRoom;
    public GameObject testSideRoom;


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

    RandomQueue<StationPart> nextLocations;
    public void Awake()
    {

       rotationList = new Quaternion[]
       {
            A, B, C, D
       };

        rotations = new Dictionary<RoomRotation, Quaternion>();
        for(int i = (int)RoomRotation.A; i <= (int) RoomRotation.D; i++)
        {
            rotations.Add((RoomRotation)i, rotationList[i]);
        }

        nextLocations = new RandomQueue<StationPart>();

        //When building the space station, start at 0
        nextLocations.Push(seedRoom.GetComponent<StationPart>());
    }


    public void Start()
    {
        BuildInterior();
    }


    public void BuildInteriorSection(StationPart part, ref RandomQueue<StationPart> nextLocations)
    {
        //Designed to deal with modified parts
        bool[] randomSelection = RandomSelection(part);

        //TODO - configure X room prefab so that this works
        for (int i = (int)(RoomRotation.A); i <= (int)(RoomRotation.D); i++)
        {
            RoomRotation configKey = (RoomRotation)i;
            RoomRotation oppositeConfigKey = StationPart.GetOppositeRotation(configKey);

            if(randomSelection[i])
            {
                Quaternion rotation = rotations[configKey]*seedRoom.transform.rotation;
                Vector3 position = part.connections[configKey].transform.position;
                part.RemoveConnection((int)configKey);

                GameObject g = Instantiate(testSideRoom, position, rotation);
                StationPart newPart = g.GetComponent<StationPart>();
                newPart.RemoveConnection((int)oppositeConfigKey);
                g.transform.localScale = seedRoom.transform.localScale;
                g.transform.parent = transform;
                nextLocations.Push(newPart);
            }
        }
    }

    public void BuildInterior(int numRooms)
    {
        this.numRooms = numRooms;
        BuildInterior();
    }
    public void BuildInterior()
    {
        int roomsRemaining = numRooms;

        while(roomsRemaining > 0) {
            roomsRemaining--;


            StationPart room = nextLocations.Pop();

            if (room.connections.Count != 0)
            {
                BuildInteriorSection(room, ref nextLocations);
            }
            else
            {
                nextLocations.RemoveLatestPop();
            }


        }

    }

    public bool[] RandomSelection(StationPart part)
    {
        //Use a diminishing returns algorithm for each section of the station.
        bool[] selection = { false, false, false, false };
        List<int> indices= new List<int> { 0, 1, 2, 3 };

        List<int> indexOrder = new List<int>();

        while(indices.Count > 0)
        {
            int index = UnityEngine.Random.Range(0, indices.Count);
            indexOrder.Add(indices[index]);
            indices.Remove(indices[index]);
        }

        float factor = 1;

        for(int i = 0; i < indexOrder.Count; i++)
        {
            if (part.ContainsIndex(i))
            {
                if (UnityEngine.Random.Range(0f, 1f) < factor)
                {
                    selection[indexOrder[i]] = true;
                    factor *= branchFactor;
                }
            }
           
        }
        return selection;
    }
}
