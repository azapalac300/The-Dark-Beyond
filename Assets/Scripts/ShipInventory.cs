using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInventory : MonoBehaviour
{
    // Start is called before the first frame update
    public void Initialize()
    {
        PlayerSpaceInput.InventoryButtonPressed += Open;
    }

    private bool isActive;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
        isActive = false;
    }

    public void Open()
    {

        gameObject.SetActive(true);
        isActive = true;
    }

    public void Toggle()
    {
        if (isActive)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
}
