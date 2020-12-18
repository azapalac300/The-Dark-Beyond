using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class TerrainData: ScriptableObject
{
    //Terrain data is not used for gas giants so all planets must be terrestrial
    //use this to pass the object into the scene rather than use singletons.
    public Vector3 landingPosition;

    [SerializeField]
    public PlanetSkeleton skeleton;
}
