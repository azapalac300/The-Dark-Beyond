using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SpaceObjectType
{
    Star,
    BlackHole,
    Nebula,
    Anomaly
}

[CreateAssetMenu]
public class SpaceObjectFactory : ScriptableObject
{
    [SerializeField]
    public List<GameObject> ghosts;

    [SerializeField]
    public List<Color> starlightColors;


    public GameObject CreateSpaceObject(Vector3 position, int key, SpaceObjectType s)
    {
        GameObject g = Instantiate(ghosts[(int)s], position, Quaternion.identity);
        g.GetComponent<SpaceGhost>().key = key;
        return g;
    }

}
 