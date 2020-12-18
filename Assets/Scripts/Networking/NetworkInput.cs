using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkInput : MovementInput {
    public int playerNum;

    public float distanceThresh;
    public float angleThresh;

    public void Update()
    {
        
    
       Vector3 targetPosition = NetworkData.GetPlayerData(playerNum).positionData;
       Quaternion targetRotation = NetworkData.GetPlayerData(playerNum).rotationData;
       float moveDelta = Vector3.Distance(transform.position, targetPosition);
       float rotateDelta = Quaternion.Angle(transform.rotation, targetRotation);

       if(moveDelta < distanceThresh)
        {
            //moveDelta = distanceThresh;
            transform.position = targetPosition;
        }
   
        //Debug.Log("Move Delta: " + moveDelta);

       if(rotateDelta < angleThresh)
        {
            transform.rotation = targetRotation;
        }

       transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateDelta * Time.deltaTime);



        transform.position = Vector3.MoveTowards(transform.position, targetPosition,  moveDelta * moveDelta * Time.deltaTime);
    }



}
