using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encoder;
public class ClientSendData : MonoBehaviour
{
    public static ClientSendData instance;
    private void Awake()
    {
        instance = this;
    }

    public void SendPacket(Packet packet, Channel channel)
    {
        SendDataToServer(DataEncoder.EncodePacket(packet), channel);
    }

    private void SendDataToServer(byte[] data, Channel channel)
    {
        if (Network.instance.isConnected)
        {
            try
            {
                Network.instance.myStream[(int)channel].Write(data, 0, data.Length);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Network not connected!");
        }
    }

    public void SayHello()
    {
        string message = "Hello!";
        SendPacket(new StringPacket(message), Channel.connect);

    }
}