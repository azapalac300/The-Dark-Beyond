using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlanetFactory : ScriptableObject {
    [SerializeField]
    public GameObject terrestrialDefault;

    [SerializeField]
    public GameObject spaceStationDefault;

    [SerializeField]
    public GameObject gasGiantDefault;

    [SerializeField]
    public GameObject moonDefault;

    [SerializeField]
    public List<Material> gasGiantSurfaces;

    public GameObject Terrestrial()
    {

        GameObject terrestrialSeed = new GameObject();
        Component[] components = terrestrialDefault.GetComponents<Component>();

        for(int i = 0; i < components.Length; i++)
        {
            Component dummyComponent = null;
            if (!terrestrialSeed.TryGetComponent(components[i].GetType(), out dummyComponent))
            {
                terrestrialSeed.AddComponent(components[i].GetType());
            }
           
        }

        return terrestrialSeed;
    }

    public GameObject GasGiant()
    {
        GameObject g = gasGiantDefault;
        g.GetComponent<MeshRenderer>().material = gasGiantSurfaces[Random.Range(0, gasGiantSurfaces.Count)];
        return g;
    }

    public GameObject Moon()
    {
        return moonDefault;
    }

    public GameObject SpaceStation()
    {
        return spaceStationDefault;
    }


}
