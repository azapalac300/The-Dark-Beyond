using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Handle events 
public class PlayerSpaceInput : MonoBehaviour {
    public static bool A, B, X, Y, RT, LT, RB, LB;

    public ControlsData data;

    //UI actions
    public static Action MapButtonPressed;
    public static Action InventoryButtonPressed;
    public static Action JournalButtonPressed;
    public static Action CharactersButtonPressed;
    public static Action ShipButtonPressed;

    public static event Action SelectButtonPressed;

    public static event Action PrimaryFired;
    public static event Action PrimaryNotFired;

    public static event Action SecondaryFired;
    public static event Action SecondaryNotFired;

    public static event Action Ability0Fired;
    public static event Action Ability1Fired; 
    public static event Action Ability2Fired;

    public static Vector3 cursorPosition;


    public void Update()
    {
        ButtonInput buttonInput = GetButtonInput();
        A = buttonInput.aPressed;
        B = buttonInput.bPressed;
        X = buttonInput.xPressed;
        Y = buttonInput.yPressed;


        if (A)
        {
            PrimaryFired?.Invoke();
        }
        else
        {
            PrimaryNotFired?.Invoke();
        }

        cursorPosition = Input.mousePosition;

        HandleMapButton();
        HandleSelectButton();


    }

  

    public void FireSecondary()
    {
        SecondaryFired?.Invoke();
    }



    public void HandleMapButton()
    {
        if (Input.GetKeyDown(data.mapKey)) {
            MapButtonPressed?.Invoke();
        }
    }

    public void HandleInventoryButton()
    {
        if (Input.GetKeyDown(data.invKey))
        {
            InventoryButtonPressed?.Invoke();
        }
    }


    public void HandleSelectButton()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectButtonPressed?.Invoke();
        }

    }

    public Vector3 GetMovementInput()
    {
        float x = Input.GetAxis("Horizontal");

        float z = Input.GetAxis("Vertical");

        return new Vector3(x, 0, z);

    }

    public ButtonInput GetButtonInput()
    {
        ButtonInput buttonInput = new ButtonInput();
        buttonInput.aPressed = (Input.GetAxis("Fire1") == 1 || Input.GetKey(KeyCode.F));
        buttonInput.bPressed = (Input.GetAxis("Fire2") == 1);
        buttonInput.xPressed = (Input.GetAxis("Fire3") == 1);
        buttonInput.yPressed = (Input.GetAxis("Fire4") == 1);
        return buttonInput;
    }
}


 public class ButtonInput
{
    public bool aPressed, bPressed, xPressed, yPressed;
}