using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    private static GameObject ObjectFolder;

    [SerializeField]
    private GameObject objectFolder;
    // Start is called before the first frame update
    private void Awake()
    {
        ObjectFolder = objectFolder;
    }

    private void OnEnable()
    {
        if (!objectFolder.activeSelf)
        {
            //SetActive();
        }
    }
    public void SetInactive()
    {
        ObjectFolder.SetActive(false);
    }

    public void SetActive()
    {
        ObjectFolder.SetActive(true);

    }
}
