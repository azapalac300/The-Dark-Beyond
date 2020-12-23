using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomRotation
{
    A, B, C, D
}



public class StationPart : MonoBehaviour
{
    public GameObject markerA, markerB, markerC, markerD;
    public List<GameObject> markers
    {
        get
        {
            return new List<GameObject> { 
            markerA,
            markerB,
            markerC,
            markerD,
            };
        }
    }

    public Dictionary<RoomRotation, GameObject> connections;


    public static RoomRotation GetOppositeRotation(RoomRotation rotation)
    {

        switch (rotation)
        {
            case RoomRotation.A:
                return RoomRotation.C;
                
            case RoomRotation.B:
                return RoomRotation.D;
                
            case RoomRotation.C:
                return RoomRotation.A;
                
            case RoomRotation.D:
                return RoomRotation.B;
        }

        return RoomRotation.A;
    }

    public bool ContainsIndex(int index)
    {
         
        return connections.ContainsKey((RoomRotation)index);
    }

    public void RemoveConnection(int index)
    {
        
        connections.Remove((RoomRotation)index);
    }

    private void Awake()
    {
        connections = new Dictionary<RoomRotation, GameObject>();
        for(int i = 0; i < markers.Count; i++)
        {
            if(markers[i] != null)
            {
                connections.Add((RoomRotation)i, markers[i]);
            }
        }

      
    }






}
