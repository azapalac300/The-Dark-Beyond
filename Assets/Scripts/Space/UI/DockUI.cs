using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum DockUIState
{
    traveling,
    orbiting,
    landed
}


public class DockUI : MonoBehaviour {

    public static DockUIState uiState;


    public GameObject dockInfoWindow;

    public bool initialized;

    // Start is called before the first frame update
    public void Initialize()
    {
        
        Player.OnDock += ShowUI;
        Player.OnUndock += HideUI;
    }

    // Update is called once per frame
    void Update()
    {
     

    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public void ShowInfo()
    {
        dockInfoWindow.SetActive(true);
    }

    public void HideInfo()
    {

        dockInfoWindow.SetActive(false);
    }

  
}
