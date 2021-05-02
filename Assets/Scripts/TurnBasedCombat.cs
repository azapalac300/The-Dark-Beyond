using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TurnBasedCombat : MonoBehaviour
{
    public GameObject enemyPrefab;

    public Vector3 enemyPosition { get { return new Vector3(30, 0, 0); } }

    public GameObject playerObject;
    public GameObject enemyObject;



    private Combatant player;
    private Combatant enemy;

    public bool debug;

    const int standardAP = 2;

    int enemyAP;

    int playerAP;


    public Text resultText;
    public float resultTime;
    private float resultTimer;

    private static OverworldEnemy _overworldEnemy;


    private bool playerVictorious;
    private static bool combatFinished;
    private static bool combatStarted;

    public static event Action DestroyEnemy;

    // Start is called before the first frame update
    void Start()
    {
        playerAP = standardAP;
        enemyAP = standardAP;

        player = playerObject.GetComponent<Combatant>();

        player.TurnComplete += () =>  PlayerTurnCompleted(playerAP);


        player.Destroyed += () => CombatComplete(false);



    }


    // Update is called once per frame
    void Update()
    {
        if (combatStarted)
        {
            SetupEnemy();
            RollInitiative();
            resultText.gameObject.SetActive(false);
            combatStarted = false;
        }

        if (combatFinished)
        {
            resultTimer -= Time.deltaTime;

            if(resultTimer <= 0)
            {
                resultTimer = 0;
                //return to overworld
                GameControl.LoadSpace();
            }
        }


    }

    public void PlayerTurnCompleted(int ap)
    {
        enemy.TakeTurn(ap);
    }


    public void EnemyTurnCompleted(int ap)
    {
        player.TakeTurn(ap);
    }


    public void CombatComplete(bool victory)
    {
        resultTimer = resultTime;

        resultText.gameObject.SetActive(true);

        if (victory)
        {
            resultText.text = "Victory!";
        }
        else
        {
            resultText.text = "Defeat!";
        }

        playerVictorious = victory;

        if (playerVictorious)
        {
            DestroyEnemy?.Invoke();
            
        }

        combatFinished = true;
    }

    public static void SetupCombat(OverworldEnemy overworldEnemy)
    {
        combatFinished = false;
        combatStarted = true;
        
        DestroyEnemy += () => {
            overworldEnemy.DropLoot();
            DestroyEnemy = null; 
        };
        
        
        _overworldEnemy = overworldEnemy;
     
    }

    public void SetupEnemy()
    {
        enemyObject.SetActive(true);
        Enemy newEnemy = enemyPrefab.GetComponent<Enemy>();
        newEnemy.maxHP = _overworldEnemy.data.HP;
        enemy = newEnemy;

        enemy.TurnComplete += () => EnemyTurnCompleted(enemyAP);

        enemy.Destroyed += () => CombatComplete(true);
    }

    private void RollInitiative()
    {

        //For now the player automatically goes first;
        player.TakeTurn(playerAP);
    }
}
