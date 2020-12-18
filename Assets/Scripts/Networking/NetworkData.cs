using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encoder;

//NetworkData acts as an interface for multiple databases
public class NetworkData : MonoBehaviour {
    public static NetworkData instance;
    private static PlayerDatabase playerDatabase;
    private void Awake()
    {
        playerDatabase = new PlayerDatabase();
        instance = this;
    }
    
    public static void HandleNewPlayer(PlayerData data)
    {
        playerDatabase.AddLocalPlayer(data);
        //SpawnPlayer.instance.SpawnOtherPlayer(data);
    }

    public static void HandlePlayerMovement(PlayerMovementPacket movementPacket)
    {
        Vector3 vector = movementPacket.positionData.GetVector3();
         

         playerDatabase.UpdatePlayerMovement(movementPacket);
    }

    public static PlayerData GetPlayerData(int playerNum)
    {
        return playerDatabase.GetPlayerData(playerNum);
    }

    public static void HandleExistingPlayers(List<PlayerData> dataList)
    {
        
        for(int i = 0; i < dataList.Count; i++)
        {
            HandleNewPlayer(dataList[i]);
        }
    }

}
