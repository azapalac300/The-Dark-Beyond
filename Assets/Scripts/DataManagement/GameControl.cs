using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Encoder;
using System;


public enum Scenario
{
    Space,
    Interior,
    Ground,
    Combat
}

public interface PlayZoneControl{

    void Pause();

    void Play();
}

public class GameControl : MonoBehaviour {

    public GameObject Space;

    public GameObject Interior;

    public GameObject Ground;

    public GameObject Combat;

    public PlayZoneControl currentZone;

    public GameObject Convo; //Convo superimposes other game modes


    private static event Action LoadSpaceAction;

    private static event Action LoadInteriorAction;

    private static event Action LoadGroundAction;

    private static event Action LoadCombatAction;

    private static event Action LoadConvoAction;

    private static event Action DismissConvoAction;

    public Scenario defaultScenario;

    public void Awake()
    {

        #region setup load actions
        LoadSpaceAction += () => {
            Space.SetActive(true);
            Interior.SetActive(false);
            Ground.SetActive(false);
            Combat.SetActive(false);
        };

        LoadInteriorAction += () => {
            Space.SetActive(false);
            Interior.SetActive(true);
            Ground.SetActive(false);
            Combat.SetActive(false);


        };

        LoadGroundAction += () => {
            Space.SetActive(false);
            Interior.SetActive(false);
            Ground.SetActive(true);
            Combat.SetActive(false);
        };

        LoadCombatAction += () =>
        {
            Space.SetActive(false);
            Interior.SetActive(false);
            Ground.SetActive(false);
            Combat.SetActive(true);
            
        };

        LoadConvoAction += () =>
        {
            currentZone.Pause();
            Convo.SetActive(true);
        };

        DismissConvoAction += () =>
        {
            currentZone.Play();
            Convo.SetActive(false);
        };
        #endregion

        switch (defaultScenario)
        {
            case Scenario.Combat:
                LoadCombat();
                break;

            case Scenario.Space:
                LoadSpace();
                break;

            case Scenario.Interior:
                LoadInterior();
                break;

            case Scenario.Ground:
                LoadGround();
                break;
        }
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

    public static void LoadCombat()
    {
        LoadCombatAction?.Invoke();
    }

    public static void LoadConvo()
    {
        LoadConvoAction?.Invoke();
    }

    public static void DismissConvo()
    {
        DismissConvoAction?.Invoke();
    }

}
