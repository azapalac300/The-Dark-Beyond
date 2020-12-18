﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encoder;


public class ClientHandleData : MonoBehaviour
{
    public static ClientHandleData instance;


    private void Awake()
    {
        instance = this;
    }
    public void HandleData(byte[] data)
    {
        Packet packet = DataEncoder.DecodePacket(data);
        HandleMessages(packet);
    }

    void HandleMessages(Packet packet)
    {

        switch (packet.packetType)
        {

            case PacketType.StringPacket:
                //HandleString((StringPacket)packet);
                break;
            case PacketType.PlayerPacket:
                //HandlePlayerPacket((PlayerPacket)packet);
                break;
            default:
                Debug.LogError("Error: Unrecognized packet type" );
                break;
        }
    }
    
    void HandlePlayerPacket(PlayerPacket playerPacket)
    {
        switch (playerPacket.playerPacketType)
        {
            case PlayerPacket.PlayerPacketType.PlayerData:
               // NetworkData.HandleNewPlayer(((PlayerDataPacket)playerPacket).data);
                break;
            case PlayerPacket.PlayerPacketType.PlayerDataResponse:
                //NetworkData.HandleExistingPlayers(((PlayerDataResponsePacket)playerPacket).otherPlayerData);
                break;
            case PlayerPacket.PlayerPacketType.Movement:
                //Debug.Log("Got movement Data!");
                //NetworkData.HandlePlayerMovement((PlayerMovementPacket)playerPacket);
                break;
            default:
                Debug.LogError("Error: Unrecognized player packet type");
                break;

        }


    }



    void HandleString(StringPacket s)
    {
        
        Debug.Log(s.data);
    }

}