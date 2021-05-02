using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    public static Camera MainCamera { get; private set; }



    [SerializeField]
    private Camera mainCamera;


    // Start is called before the first frame update
    void Awake()
    {
        MainCamera = mainCamera;
    }

}
