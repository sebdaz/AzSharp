using MessagePack;
using Network;
using Network.Interfaces;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Connections;


public static class ConnectionExtension
{
    public static void SendMessage<T>(this Connection connection, T thing)
    {
        string type_name = typeof(T).Name;
        byte[] bytes = MessagePackSerializer.Serialize(thing);
        connection.SendRawData(type_name, bytes);
    }
}
