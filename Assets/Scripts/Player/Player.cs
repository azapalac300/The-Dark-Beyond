using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float moveSpeed;
    public float maneuverability;


    public PlayerData data;
    private PlayerInput input;
    private  Docking currentDock;
    public static Player instance;
    public  bool Docked { get { return currentDock != null; } }

    public  Vector3 Position
    {
        get; private set;
       
    }
  


    private void Awake()
    {
       if(instance == null)
        {
            instance = this;
        }
  
        gameObject.SetActive(true);

        input = GetComponent<PlayerInput>();

        DockUI.uiState = DockUIState.traveling;
    }


    void Start()
    {
        
        //transform.position = data.positionData;
        //transform.localRotation = Quaternion.identity;

    }




    // Update is called once per frame
    void Update () {

        if (Docked) {
            HandleDocking();         
        }
        else {
            HandleMovement();
        }
        
        UpdataData();
        ActivateCelestialObjectsNearby();


    }

    public void Dock(Docking d)
    {
        transform.parent = d.transform;
        currentDock = d;
        DockUI.uiState = DockUIState.orbiting;
        Debug.Log("Docked!");
       
    }

   public void Land()
    {
        if (Docked)
        {
            currentDock.parentPlanet.Land();
        }
    }

    public void Undock()
    {
        transform.parent = null;
        currentDock = null;
        DockUI.uiState = DockUIState.traveling;
    }

    private void HandleMovement()
    {
        if (!Docked)
        {
            Vector3 movementVector = input.GetMovementInput();


            //Rotate player based on input, only rotate on a plane
            Vector3 fwdVector = Vector3.forward * movementVector.z;

            transform.Rotate(Vector3.up, movementVector.x * maneuverability * Time.deltaTime);


            //Move player based on input
            transform.Translate(fwdVector * moveSpeed * Time.deltaTime);

            Position = transform.position;
        }
    }

    private void HandleDocking()
    {
        if (Docked)
        {

        }
    }

   public void UpdataData()
    {
        data.rotationData = transform.rotation;
        data.positionData = transform.position;

    }

    public void ActivateCelestialObjectsNearby()
    {

        Collider[] nearbyBlipColliders = Physics.OverlapSphere(transform.position, GameData.PlayRadius);
        for (int i = 0; i < nearbyBlipColliders.Length; i++)
        {
            GameObject spaceObject = nearbyBlipColliders[i].gameObject;
            if (spaceObject.tag == "Ghost")
            {
                spaceObject.GetComponent<SpaceGhost>().ActivateBlip();
            }
        }

        //Return true if at least one player is nearby
        Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, GameData.PlayRadius);
        for (int i = 0; i < nearbyColliders.Length; i++)
        {
            GameObject spaceObject = nearbyColliders[i].gameObject;
            if (spaceObject.tag == "Ghost")
            {
                spaceObject.GetComponent<SpaceGhost>().Activate();
            }
        }
    }


}
