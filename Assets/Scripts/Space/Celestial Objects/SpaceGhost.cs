using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpaceGhost : MonoBehaviour
{
    [SerializeField]
    private GameObject SpaceObject;
    private SpaceObject _spaceObject;

    public int key;

    private bool blipActive;

    public SpaceObject spaceObject
    {
        get
        {
            if(_spaceObject == null)
            {
                _spaceObject = SpaceObject.GetComponent<SpaceObject>();
            }
            return _spaceObject;
        }
    }




    public void Awake()
    {
        
        spaceObject.Initialize();
        Deactivate();
        DeactivateBlip();
    }



    public void ActivateBlip()
    {
        Starmap.SpawnStarMarker(key, spaceObject.Position);

    }

    public void DeactivateBlip()
    {
        Starmap.DespawnStarMarker(key);
    }

        public void Activate()
        {
      
        spaceObject.Activate();
        gameObject.tag = "SolarSystem";
        gameObject.name = "Active";

        }

        public void Deactivate()
        {
       
        spaceObject.Deactivate();
        gameObject.tag = "Ghost";
        gameObject.name = "Inactive";
        }
    }

