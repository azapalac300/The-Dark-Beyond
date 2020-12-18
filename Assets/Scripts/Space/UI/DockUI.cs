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

    private bool dockButtonsActive;
    public Button undock;
    public Button land;
    public Button dockInfo;

    public GameObject dockInfoWindow;

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        switch (uiState) {
            case DockUIState.landed:
                break;

            case DockUIState.orbiting:
                undock.gameObject.SetActive(true);
                land.gameObject.SetActive(true);
                dockInfo.gameObject.SetActive(true);
                break;
            case DockUIState.traveling:
                undock.gameObject.SetActive(false);
                land.gameObject.SetActive(false);
                dockInfo.gameObject.SetActive(false);
                break;

        }

        

    }


    public void ShowInfo()
    {
        if (dockInfoWindow.activeSelf)
        {
            dockInfoWindow.SetActive(false);
        }
        else
        {
            dockInfoWindow.SetActive(true);
        }
    }
}
