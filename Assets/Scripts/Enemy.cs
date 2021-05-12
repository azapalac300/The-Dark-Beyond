using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Enemy : MonoBehaviour, Combatant
{
    #region variables
    public int maxHP;
    public int currHP;
    public Image hpBar;

    //Timers
    public float beamTimeLimit;
    public float beamTimer;

    private const float gapDelta = 0.2f;

    

    private float gapTimeLimit;

    private float gapTimer;
    //____________________

    public LineRenderer beam;

    public int damage;
    public int actionPoints;

    public bool IsCurrentTurn { get; set; }

    public event Action TurnComplete;
    public event Action Destroyed;

     #endregion

    private bool Shooting {  get { return beamTimer > 0 || gapTimer > 0; } }

    // Start is called before the first frame update
    void Start()
    {
        beamTimer = 0;
      
        currHP = maxHP;
        hpBar = GameObject.Find("EnemyHP").GetComponent<Image>();
        gapTimeLimit = beamTimeLimit + gapDelta;
    }


    // Update is called once per frame
    void Update()
    {



        ManageWeapon();

        ManageTurn();


    }

    public void ManageWeapon()
    {
        if (beamTimer > 0)
        {
            beamTimer -= Time.deltaTime;


            beam.gameObject.SetActive(true);
        }
        else
        {
            beam.gameObject.SetActive(false);
        }

    }


    public void ManageTurn()
    {
        if (IsCurrentTurn)
        {
            if (actionPoints == 0)
            {
                TurnComplete?.Invoke();
            }else
            {
                if (!Shooting && gapTimer <= 0)
                {
                    Shoot();

                    if (actionPoints > 0)
                    {
                        gapTimer = gapTimeLimit;
                    }
                }

                if (gapTimer > 0)
                {
                    gapTimer -= Time.deltaTime;
                }
            }
        }

        if (gapTimer < 0)
        {
            gapTimer = 0;
        }
    }


    public void Shoot()
    {
        Vector3 p0 = beam.GetPosition(0);

        Vector3 p1 = beam.GetPosition(1);

        Vector3 dir = p0 - p1;
        Ray ray = new Ray(p1, dir);

        Debug.DrawRay(p1, dir, Color.white, 600f);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            hitObject.GetComponentInParent<Combatant>().TakeDamage(damage, DamageType.Ballistic);
        }

        beamTimer = beamTimeLimit;
        actionPoints--;
    }

    public void TakeTurn(int actionPoints)
    {
        this.actionPoints = actionPoints;
        gapTimeLimit = beamTimeLimit + gapDelta;
        gapTimer = gapTimeLimit/2;
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
