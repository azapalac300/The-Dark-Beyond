using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewUI : MonoBehaviour
{
    public List<GameObject> allRelationships;


    public bool hideRelationships;

    // Start is called before the first frame update
    void Start()
    {
        HideAllRelationships();
    }

    // Update is called once per frame
    void Update()
    {
        if (hideRelationships)
        {
            HideAllRelationships();
            hideRelationships = false;
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void CrewUIShortcut()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }


    }

    public void HideAllRelationships()
    {
        foreach (GameObject relationship in allRelationships)
        {
            relationship.SetActive(false);
        }
    }
}
