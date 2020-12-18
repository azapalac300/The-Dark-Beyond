using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrestrialGradients : ScriptableObject
{
    [SerializeField]
    public List<NamedGradient> gradients;
}

[System.Serializable]
public class NamedGradient
{
    [SerializeField]
    public string name;

    [SerializeField]
    public Gradient gradient;
}
