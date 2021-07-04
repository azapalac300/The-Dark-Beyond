using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CrewUIEntry : MonoBehaviour
{
    public bool onSelectedTest;
    
    public List<GameObject> relationships;

    public Sprite statusCircle;

    public Color selectedColor;

    private CrewUI crewUI;
    // Start is called before the first frame update
    void Start()
    {
        crewUI = GetComponentInParent<CrewUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (onSelectedTest)
        {
            OnSelected();
            onSelectedTest = false;
        }
    }


    public void OnSelected()
    {

        crewUI.HideAllRelationships();

        foreach(GameObject relationship in relationships)
        {
            relationship.SetActive(true);
        }
    }


}
