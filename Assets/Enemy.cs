using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour, Combatant
{
    
    public int maxHP;
    public int currHP;
    public Image hpBar;

    public float beamTimeLimit;
    public float beamTimer;

    public GameObject beam;

    public int actionPoints;

   public bool IsCurrentTurn { get; private set; }

    public event Action TurnComplete;

    // Start is called before the first frame update
    void Start()
    {
        beamTimer = 0;
        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = (float)currHP / (float)maxHP;



        if(beamTimer > 0)
        {
            beamTimer -= Time.deltaTime;


            beam.SetActive(true);
        }
        else
        {
            beam.SetActive(false);
        }

        if(actionPoints == 0 && IsCurrentTurn)
        {
            IsCurrentTurn = false;
            TurnComplete?.Invoke();
        }
    }

    public void TakeDamage(int damageToTake)
    {
        currHP -= damageToTake;
        if(currHP <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        beamTimer = beamTimeLimit;
        actionPoints--;
    }

    public void TakeTurn(int actionPoints)
    {
        IsCurrentTurn = true;
        this.actionPoints = actionPoints;
    }

    public void TakeDamage(int damageAmount, DamageType damageType)
    {

        currHP -= damageAmount;

    }
}
