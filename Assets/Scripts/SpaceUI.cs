using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceUI : MonoBehaviour
{
    //Used to manage UI stuff at a top level
    public Starmap starmap;
    void Start()
    {
        starmap.Initialize();
    }
}
