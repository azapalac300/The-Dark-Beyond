using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Encoder;
using System;

public class GameControl : MonoBehaviour {
    public GameObject Space;

    public GameObject Interior;

    public GameObject Ground;

    private static event Action LoadSpaceAction;

    private static event Action LoadInteriorAction;

    private static event Action LoadGroundAction;

    public void Awake()
    {
        LoadSpaceAction += () => {
            Space.SetActive(true);
            Interior.SetActive(false);
            Ground.SetActive(false);

        };

        LoadInteriorAction += () => {
            Space.SetActive(false);
            Interior.SetActive(true);
            Ground.SetActive(false);
        };

        LoadGroundAction += () => {
            Space.SetActive(false);
            Interior.SetActive(false);
            Ground.SetActive(true);
        };

        LoadInterior();
    }

    public static void LoadSpace()
    {
        LoadSpaceAction?.Invoke();
    }

    public static void LoadInterior()
    {
        LoadInteriorAction?.Invoke();
    }

    public static void LoadGround()
    {
        LoadGroundAction?.Invoke();
    }
}
