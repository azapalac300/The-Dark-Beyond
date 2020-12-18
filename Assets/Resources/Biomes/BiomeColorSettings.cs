using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu][System.Serializable]
public class BiomeColorSettings : ScriptableObject
{
    //Biome space exists in 1 dimension for now. Will turn into a cube later

    [SerializeField]
    public BiomeResolution biomeResolution;


    public int xResolution { get { return biomeResolution.resolution; } }
    public int yResolution { get { return 1; } }
    public int zResolution { get { return 1; } }


    [SerializeField]
    public Biome[] biomes;

}

[System.Serializable]
public class Biome
{
    [SerializeField]
    public Gradient gradient;
}
