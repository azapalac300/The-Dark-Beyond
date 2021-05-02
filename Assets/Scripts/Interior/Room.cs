using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP, RIGHT, DOWN, LEFT
}


public enum RoomType
{
    X, T, L, I, U, O
}


public enum RoomFunction
{
    Standard, Refuel, Bar, PartShop
}

public class Room : MonoBehaviour
{
    public GameObject upCorridor, rightCorridor, downCorridor, leftCorridor;

    private List<GameObject> corridors;

    public RoomType roomType;
    public bool centerFilled;
    public RoomFunction roomFunction;
    public string coords;
    public List<Room> neighbors;

    private int coordX, coordY;
    public List<Direction> availableDirections;




    public void Initialize()
    {
        roomFunction = RoomFunction.Standard;
        availableDirections = new List<Direction>() {
        Direction.UP,
        Direction.RIGHT,
        Direction.DOWN,
        Direction.LEFT
        };

        corridors = new List<GameObject>()
        {
            upCorridor, rightCorridor, downCorridor, leftCorridor
        };

        neighbors = new List<Room>();
    }

    private void Start()
    {
        gameObject.name = coords;
    }

    public Room SpawnNeighboringRoom(GameObject roomPrefab, Direction direction, float offset, Transform parent, ref Dictionary<string, Room> roomDict )
    {
        
        Vector3 spawnPosition = transform.position;
        Vector3 dir = Vector3.zero;
        int x = coordX;
        int y = coordY;

        switch (direction)
        {
            case Direction.UP:
                y++;
                dir = Vector3.forward;
                break;
            case Direction.RIGHT:
                x++;
                dir = Vector3.right;
                break;
            case Direction.DOWN:
                y--;
                dir = Vector3.back;
                break;
            case Direction.LEFT:
                x--;
                dir = Vector3.left;
                break;
        }

        string coords = MakeCoords(x, y);
        if (roomDict.ContainsKey(coords))
        {
            return null;
        }

        spawnPosition += (dir * offset);

       
        GameObject newRoomObject = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
        newRoomObject.transform.SetParent(parent);
        Room newRoom = newRoomObject.GetComponent<Room>();
        newRoom.Initialize();
        newRoom.SetCoords(x, y);
      

        roomDict.Add(coords, newRoom);
        return newRoom;
    }

    public void FindRoomsNearby(Dictionary<string, Room> roomDict)
    {
        for(int i = 0; i < 4; i++)
        {

            Direction direction = (Direction)i;

            if (availableDirections.Contains(direction))
            {
                int x = coordX;
                int y = coordY;
                switch (direction)
                {
                    case Direction.UP:
                        y++;
                        break;
                    case Direction.DOWN:
                        y--;
                        break;
                    case Direction.LEFT:
                        x--;
                        break;
                    case Direction.RIGHT:
                        x++;
                        break;

                }

                string neighborCoords = MakeCoords(x, y);

                if (roomDict.ContainsKey(neighborCoords))
                {
                    
                    Room neighbor = roomDict[neighborCoords];

                    neighbors.Add(neighbor);
                    neighbor.neighbors.Add(this);
                    GetCorridor(direction).SetActive(true);
                    availableDirections.Remove(direction);
                    neighbor.availableDirections.Remove(GetOpposite(direction));
                    roomDict[neighborCoords] = neighbor;
                }
            }
        }
    }


    public void RemoveDirection(Direction direction)
    {
        availableDirections.Remove(direction);

    }

    private Direction GetOpposite(Direction direction)
    {
        switch(direction)
        {
            case Direction.UP:
                return Direction.DOWN;

            case Direction.DOWN:
                return Direction.UP;

            case Direction.LEFT:
                return Direction.RIGHT;

            case Direction.RIGHT:
                return Direction.LEFT;
        }


        //This code cannot be reached but C# needs it anyway
        return Direction.UP;
    }

    public GameObject GetCorridor(Direction direction)
    {
        switch (direction)
        {
            case Direction.UP:
                return upCorridor;
            case Direction.DOWN:
                return downCorridor;
            case Direction.LEFT:
                return leftCorridor;
            case Direction.RIGHT:
                return rightCorridor;
        }
        return null;
    }

    public void DetermineRoomType() {

        switch (availableDirections.Count)
        {

            case 0:
                roomType = RoomType.X;
                break;

            case 1:
                roomType = RoomType.T;
                break;

            case 2:
                if(GetOpposite(availableDirections[0]) == availableDirections[1])
                {
                    roomType = RoomType.I;
                }
                else
                {
                    roomType = RoomType.L;
                }
               
                break;


            case 3:
                roomType = RoomType.U;
                break;

            case 4:
                roomType = RoomType.O;
                break;

        }



    }


    public void FurnishRoom(GameObject floorStyle, GameObject corridorStyle)
    {


        Vector3 corridorDelta = Vector3.up;
        GameObject floor = Instantiate(floorStyle, transform.position, Quaternion.identity);

        Vector3 floorScale = floorStyle.transform.localScale;
        floor.transform.localScale = new Vector3(transform.localScale.x * floorScale.x, 1, transform.localScale.z*floorScale.z);
        floor.transform.localPosition = transform.position + new Vector3(floor.transform.localScale.x * 2.5f, 1, -1 * floor.transform.localScale.z * 2.5f);   
          
        floor.name = "Floor";
        //floor.transform.SetParent(transform);


        for(int i = 0; i < corridors.Count; i++)
        {
           // if (corridors[i].activeInHierarchy)
           // {
                GameObject corridor = Instantiate(corridorStyle, corridors[i].transform.position + corridorDelta, Quaternion.identity);
                corridor.transform.localScale = transform.localScale;
                corridor.transform.Translate(transform.localScale.x, 0, -transform.localScale.z);
                corridor.name = "Corridor";
                corridor.transform.SetParent(transform);
           // }
        }

    }

    private string MakeCoords(int x, int y)
    {
        return x + " " + y;
    }

    public string SetCoords(int x, int y)
    {
        coordX = x;
        coordY = y;
        coords = MakeCoords(x, y);
        return coords;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
