using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HandleScrollbar : MonoBehaviour
{
    public Slider slider;
    public GameObject handle;
    private Vector3 origHandlePosition;
    public float movementFactor;


    // Start is called before the first frame update
    void Start()
    {
        origHandlePosition = handle.transform.position;
        slider.onValueChanged.AddListener(Scroll);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Scroll(float scrollValue)
    {
        Vector3 delta = new Vector3(0, movementFactor * scrollValue, 0);

        handle.transform.position = origHandlePosition + delta;
    }
}

