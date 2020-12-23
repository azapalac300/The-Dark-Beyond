using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Starmap : MonoBehaviour
{
    public GameObject starMarkerPrefab;
    public GameObject playerMarker;
    public GameObject jumpTarget;
    public GameObject jumpButton;

    
    
    public float scaleFactor;
    public Player player;
    
    public Dictionary<int, StarMarker> markerDict;
    public Dictionary<int, Vector3> origStarPositions;
    public int selected;



    private Vector3 rectZero { get { return playerMarker.transform.position; } }

    //Event inputs
    private static Action<int, Vector3> _spawnStarMarkerAction;

    private static Action<int> _despawnStarMarkerAction;

    private static Action<int> _selectDestinationAction;

    //Event outputs
    public static event Action<Vector3> JumpEvent;

    public static void SpawnStarMarker(int key, Vector3 StarPosition)
    {
        _spawnStarMarkerAction?.Invoke(key, StarPosition);
    }

    public static void DespawnStarMarker(int key)
    {
        _despawnStarMarkerAction?.Invoke(key);
    }


    public static void SelectDestination(int key)
    {
        _selectDestinationAction?.Invoke(key);
    }

    
   public void Initialize()
    {
        origStarPositions = new Dictionary<int, Vector3>();
        markerDict = new Dictionary<int, StarMarker>();
        _spawnStarMarkerAction += SpawnStarMarkerLocal;
        _despawnStarMarkerAction += DespawnStarMarkerLocal;
        _selectDestinationAction += SelectDestinationLocal;

        PlayerSpaceInput.MapButtonPressed += () => Toggle();
        PlayerSpaceInput.SelectButtonPressed += () => CheckMapSelection();
        jumpTarget.SetActive(false);
        jumpButton.SetActive(false);
        gameObject.SetActive(false);

        selected = -1;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStarPositions();
        UpdatePlayerRotation();
        UpdateSelectedPosition();
        
    }

    void UpdatePlayerRotation()
    {

        Vector3 eulers = new Vector3(0, 0,  -player.transform.rotation.eulerAngles.y);
        playerMarker.transform.rotation = Quaternion.Euler(eulers);
    }

    void UpdateStarPositions()
    {
        foreach (int key in markerDict.Keys)
        {
            markerDict[key].transform.position = GetMarkerPos(origStarPositions[key]);
        }
    }

    void UpdateSelectedPosition()
    {
        if (markerDict.ContainsKey(selected))
        {
            jumpTarget.transform.position = markerDict[selected].transform.position;
        }
    }

    void SpawnStarMarkerLocal(int key, Vector3 starPos)
    {
        if (!markerDict.ContainsKey(key))
        {
            Vector3 markerPos = GetMarkerPos(starPos);

            GameObject marker = Instantiate(starMarkerPrefab, markerPos, Quaternion.identity);
            marker.transform.SetParent(transform);

            StarMarker starMarker = marker.GetComponent<StarMarker>();
            starMarker.Initialize(key);

            //Add to lists for bookeeping;
            markerDict.Add(key, starMarker);
            origStarPositions.Add(key, starPos);
        }
      
    }

    public void CheckMapSelection()
    {
        foreach(StarMarker starMarker in markerDict.Values)
        {
            starMarker.CheckIfSelected(PlayerSpaceInput.cursorPosition);
        }
    }

    void SelectDestinationLocal(int key)
    {
        jumpTarget.SetActive(true);
        jumpButton.SetActive(true);
        selected = key;
    }



    private Vector3 GetMarkerPos(Vector3 starPos)
    {
        return (new Vector3(starPos.x, starPos.z, 0) - new Vector3(player.Position.x, player.Position.z, 0))* scaleFactor + rectZero;
    }

    void DespawnStarMarkerLocal(int key)
    {
        if (markerDict.ContainsKey(key))
        {
            GameObject.Destroy(markerDict[key].gameObject);

            markerDict.Remove(key);
            origStarPositions.Remove(key);
        }
    }


    public void JumpButtonPressed()
    {
        if (markerDict.ContainsKey(selected))
        {
            JumpEvent?.Invoke(origStarPositions[selected]);
            selected = -1;
            jumpButton.SetActive(false);
            jumpTarget.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    public void Toggle()
    {
        if (gameObject.activeSelf)
        {
            Close();
        }
        else
        {
            Open();
        }
    }


    public void Close()
    {
        gameObject.SetActive(false);
    }


    public void Open()
    {
        gameObject.SetActive(true);
    }
}