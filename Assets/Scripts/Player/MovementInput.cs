using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementInput : MonoBehaviour {

    public virtual Vector3 GetMovementInput()
    {
        Debug.Log("Error: Calling the base MovementInput function. Use its children instead");
        return Vector3.zero;

    }

    public virtual Vector3 GetPositionInput()
    {
        return Vector3.zero;
    }
}
