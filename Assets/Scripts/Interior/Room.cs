using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    A, B, C, D
}

public enum RoomType
{
    Standard, Refuel, Bar, PartShop
}

public class Room : MonoBehaviour
{

    public List<Direction> availableDirections;
    public bool centerFilled;
    public RoomType roomType;

    // Start is called before the first frame update
    void Awake()
    {
        roomType = RoomType.Standard;
        availableDirections = new List<Direction>() {
        Direction.A,
        Direction.B,
        Direction.C,
        Direction.D

    };
}

   public Room SpawnNeighboringRoom(GameObject roomPrefab, Direction direction, float offset)
    {
        Vector3 spawnPosition = transform.position;
        Vector3 dir = Vector3.zero;


        switch (direction)
        {
            case Direction.A:
                dir = Vector3.forward;
                break;
            case Direction.B:
                dir = Vector3.left;
                break;
            case Direction.C:
                dir = Vector3.back;
                break;
            case Direction.D:
                dir = Vector3.right;
                break;
        }

        spawnPosition += (dir * offset);
        GameObject newRoomObject = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
        Room newRoom = newRoomObject.GetComponent<Room>();
        availableDirections.Remove(direction);
        newRoom.availableDirections.Remove(GetOpposite(direction));
        return newRoom;
    }

    private Direction GetOpposite(Direction direction)
    {
        Direction d = Direction.A;
        switch (direction)
        {
            case Direction.A:
                d = Direction.C;
                break;
            case Direction.B:
                d = Direction.D;
                break;
            case Direction.C:
                d = Direction.A;
                break;
            case Direction.D:
                d = Direction.B;
                break;
        }

        return d;
    }

    public void MakeSpecialRoom()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
