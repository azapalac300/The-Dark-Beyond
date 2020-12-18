using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StarMarker : MonoBehaviour
{
    public int key;

    private CircleCollider2D collider2d;
    private bool cleanedUp;
    private Action checkSelectedAction;

    // Start is called before the first frame update
    public void Initialize(int key)
    {
        this.key = key;
       
    }



    private void Awake()
    {
        collider2d = GetComponent<CircleCollider2D>();

    }

    public void CheckIfSelected(Vector3 cursorPosition)
    {

            float dist = Vector3.Distance(transform.position, cursorPosition);

            if (dist < collider2d.radius)
            {
                Starmap.SelectDestination(key);
            }

    }


}
