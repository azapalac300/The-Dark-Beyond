using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCrewManager : MonoBehaviour
{
    public InteriorBuilder interiorBuilder;

    public Vector3 spawnPoint;

    public int nCrew;


    // Start is called before the first frame update
    void Start()
    {
        if (interiorBuilder.originRoom != null) {
            spawnPoint = interiorBuilder.originRoom.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnCrew()
    {
        //Spawn the crew in the interior location


    }
}
