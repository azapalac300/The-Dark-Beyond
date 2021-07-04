using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceUI : MonoBehaviour
{
    //Used to manage UI stuff at a top level
    public Starmap starmap;
    public ShipInventory shipInventory;
    public DockUI dockUI;
    [Space(10)]
    public GameObject shortcuts;
    public GameObject shortcutsButton;
    void Start()
    {
        starmap.Initialize();
        shipInventory.Initialize();
        dockUI.Initialize();
    }

    private void Update()
    {
        
    }

    #region shortcuts
    private void HandleShortcutsMenu()
    {

    }

    public void DisplayShortcuts()
    {
        shortcutsButton.SetActive(false);
        shortcuts.SetActive(true);
    }

    public void HideShortcuts()
    {
        shortcutsButton.SetActive(true);
        shortcuts.SetActive(false);
    }

    public void MapShortcut()
    {
        HideShortcuts();

        PlayerSpaceInput.MapButtonPressed.Invoke();
    }

    public void InventoryShortcut()
    {
        HideShortcuts();

        PlayerSpaceInput.InventoryButtonPressed?.Invoke();
    }

    #endregion

}
