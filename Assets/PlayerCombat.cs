using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public enum DamageType
{
    Ballistic, Fire, Radiant, Ice, Electric, Bio
}
public interface Combatant
{
    void TakeTurn(int actionPoints);

    void Shoot();

    event Action TurnComplete;

    bool IsCurrentTurn { get; }

    void TakeDamage(int damageAmount, DamageType damageType);
}
public class PlayerCombat : MonoBehaviour, Combatant
{
    
    public int maxHP;
    public int currHP;
    public Image hpBar;

    public float beamTimeLimit;
    private float beamTimer;

    public LineRenderer beam;

    public int actionPoints;

    public Text actionPointDisplay;

    public bool IsCurrentTurn { get; private set; }
    public event Action TurnComplete;
    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
        IsCurrentTurn = true;
    }

    // Update is called once per frame
    void Update()
    {

        actionPointDisplay.text = "AP: " + actionPoints;

        hpBar.fillAmount = (float)currHP / (float)maxHP;

        if (beamTimer > 0)
        {
            beamTimer -= Time.deltaTime;


            beam.gameObject.SetActive(true);


            
        }
        else
        {
            beam.gameObject.SetActive(false);
        }

        if(actionPoints == 0 && IsCurrentTurn)
        {
            TurnComplete?.Invoke();
            IsCurrentTurn = false;
        }
    }

    public void TakeDamage(int damageToTake)
    {
        currHP -= damageToTake;
        if (currHP <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot()
    {
        if (IsCurrentTurn)
        {
            beamTimer =

                beamTimeLimit;

            actionPoints--;


            Vector3 p0 = beam.GetPosition(0);

            Vector3 p1 = beam.GetPosition(1);

            Vector3 dir = p1 - p0;
            Ray ray = new Ray(p0, dir);

            Debug.DrawRay(p0, dir, Color.white, 600f);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                hit.collider.gameObject.GetComponent<Combatant>().TakeDamage(20, DamageType.Ballistic);
            }

        }
    }

    public void TakeTurn(int actionPoints)
    {
        this.actionPoints = actionPoints;
        IsCurrentTurn = true;
    }
    

    public void TakeDamage(int damageAmount, DamageType damageType)
    {

        currHP -= damageAmount;

    }
  
}
