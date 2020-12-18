﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using Encoder;
using System.Text;

public class Network : MonoBehaviour
{
    public static Network instance;
    //The main Connection network
    //Extended to connect on multiple channels. Using magic numbers for now, will switch to putting things in a DLL if it works
    [Header("Network Settings")]
    public string ServerIP = "127.0.0.1"; //Using localhost currently
    public bool isConnected;
    public bool debugNetwork;
    public TcpClient[] playerSockets;
    public NetworkStream[] myStream;
    public StreamReader myReader;
    public StreamWriter myWriter;

    private byte[][] asyncBuf;
    private byte[][] myBytes;
    public bool[] shouldHandleData;


    private void Awake()
    {
        isConnected = false;
        instance = this;
        //ServerPort = new int[] { 5500, 5501 };
        playerSockets = new TcpClient[NetworkChannels.Count];
        myStream = new NetworkStream[NetworkChannels.Count];
        asyncBuf = new byte[NetworkChannels.Count][];
        myBytes = new byte[NetworkChannels.Count][];
        shouldHandleData = new bool[NetworkChannels.Count];
        for (int i = 0; i < NetworkChannels.Count; i++)
        {
            ConnectToGameServer(i);

        }

    }

    private void Update()
    {
        //Send message to server
        if (Input.GetKeyDown(KeyCode.Space) && debugNetwork)
        {
            ClientSendData.instance.SayHello();
        }

        for(int i = 0; i < shouldHandleData.Length; i++)
        if (shouldHandleData[i])
        {
            ClientHandleData.instance.HandleData(myBytes[i]);
            shouldHandleData[i] = false;
        }
    }


    void ConnectToGameServer( int i)
    {
        if (playerSockets[i] != null)
        {
            if (playerSockets[i].Connected || isConnected)
            {
                return;
            }
            playerSockets[i].Close();
            playerSockets[i] = null;
        }

        playerSockets[i] = new TcpClient();
        playerSockets[i].ReceiveBufferSize = 4096;
        playerSockets[i].SendBufferSize = 4096;
        playerSockets[i].NoDelay = false;
        Array.Resize(ref asyncBuf[i], 8192);
        playerSockets[i].BeginConnect(ServerIP, NetworkChannels.GetChannelPort(i), new AsyncCallback(ConnectCallback), i);
        //isConnected = true;


        // MenuManager.instance._menu = MenuManager.Menu.Home;
    }

    void ConnectCallback(IAsyncResult result)
    {
        int i = (int)result.AsyncState;
        TcpClient playerSocket = playerSockets[i];
        if (playerSockets != null)
        {
            playerSocket.EndConnect(result);
            if (playerSocket.Connected == false)
            {
                isConnected = false;
                return;
            }
            else
            {
                playerSocket.NoDelay = true;
                myStream[i] = playerSocket.GetStream();
                myStream[i].BeginRead(asyncBuf[i], 0, 8192, new AsyncCallback( OnRecieve), i);

                //Spawn once I KNOW i'm connected on all channels
                if (i == playerSockets.Length - 1)
                {
                    isConnected = true;
                }

            }

        }

    }

    void OnApplicationQuit()
    {
        //Send logout message to server
        //ClientSendData.instance.SendPacket(new Packet(PacketType.Logout), Channel.connect);
        isConnected = false;
        for(int i = 0; i < playerSockets.Length; i++)
        {
            playerSockets[i].Close();
        }
    }

    void OnRecieve(IAsyncResult result)
    {
        int i = (int)result.AsyncState;
        if (playerSockets[i] != null)
        {
            if (playerSockets[ i] == null)
            {
                return;
            }

            int byteArray = myStream[i].EndRead(result);
            myBytes[i] = null;
            Array.Resize(ref myBytes[i], byteArray);
            Buffer.BlockCopy(asyncBuf[i], 0, myBytes[i], 0, byteArray);

            if (byteArray == 0)
            {
                Debug.Log("You got disconnected from the server");
                playerSockets[i].Close();
                return;
            }
            shouldHandleData[i] = true;

            if (playerSockets[i] == null)
            {
                return;
            }

            
            myStream[i].BeginRead(asyncBuf[i], 0, 8192, new AsyncCallback(OnRecieve), i);
        }
    }


}