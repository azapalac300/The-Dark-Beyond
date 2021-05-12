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

    event Action Destroyed;

    bool IsCurrentTurn { get; set; }
    

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

    public int damage;

    public bool IsCurrentTurn { get; set; }
    public event Action TurnComplete;
    public event Action Destroyed;
    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
    }




    // Update is called once per frame
    void Update()
    {

        actionPointDisplay.text = "AP: " + actionPoints;



        if (beamTimer > 0)
        {
            beamTimer -= Time.deltaTime;


            beam.gameObject.SetActive(true);


            
        }
        else
        {
            beam.gameObject.SetActive(false);
        }

        if(actionPoints == 0 && IsCurrentTurn && beamTimer <= 0)
        {
            
            IsCurrentTurn = false;
            TurnComplete?.Invoke();
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
                GameObject hitObject = hit.collider.gameObject;
                hitObject.GetComponentInParent<Combatant>().TakeDamage(damage, DamageType.Ballistic);
            }

        }
    }

    public void TakeTurn(int actionPoints)
    {
        this.actionPoints = actionPoints;
    }
    

    public void TakeDamage(int damageAmount, DamageType damageType)
    {

        currHP -= damageAmount;
        if (currHP <= 0f)
        {
            Destroy(gameObject);
            Destroyed?.Invoke();
        }


            hpBar.fillAmount = (float)currHP / (float)maxHP;
        
    }
  
}
