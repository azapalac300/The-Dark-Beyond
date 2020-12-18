using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialObject : MonoBehaviour {
    private SpaceGhost ghost;

	// Use this for initialization
	void Awake () {
        ghost = transform.parent.GetComponent<SpaceGhost>();


	}
	
	// Update is called once per frame
	protected virtual void Update () {

        if (!PlayerNearby(1.5f))
        {
            ghost.Deactivate();
            ghost.DeactivateBlip();
        }
    }

    private bool PlayerNearby(float factor)
    {
            if(Vector3.Distance(transform.position, Player.instance.Position) < GameData.PlayRadius*factor)
            {
                return true;
            }
            else
            {
                return false;
            }
    }
}
