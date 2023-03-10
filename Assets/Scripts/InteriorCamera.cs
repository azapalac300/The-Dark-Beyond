using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorCamera : MonoBehaviour
{
    public int panSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        PanCamera();
    }

    void PanCamera()
    {

        Vector3 panVector = Vector3.zero;

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        //Handle horizontal movement
       if(mouseX >= 5 * Screen.width / 6)
        {
            panVector += Vector3.right;
        }

       if(mouseX <= 1 * Screen.width/6)
        {
            panVector += Vector3.left;
        }

        //Handle vertical movement
        if (mouseY >= 5 * Screen.height / 6)
        {
            panVector += transform.InverseTransformDirection(new Vector3(0, 0, 1));
        }

        if (mouseY <= 1 * Screen.height/6)
        {
            panVector += transform.InverseTransformDirection(new Vector3(0, 0, -1));
        }

        transform.Translate(panVector * panSpeed * Time.deltaTime);

    }
}
