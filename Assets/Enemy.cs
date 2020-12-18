using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    
    public int maxHP;
    public int currHP;
    public Image hpBar;

    // Start is called before the first frame update
    void Start()
    {
        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = (float)currHP / (float)maxHP;
    }

    public void TakeDamage(int damageToTake)
    {
        currHP -= damageToTake;
        if(currHP <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
