using MessagePack;
using Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

public abstract class NetMessageHandler<T> : INetMessageHandler
{
    public void ReceiveMessage(byte[] data, Connection connection)
    {
        T message = MessagePackSerializer.Deserialize<T>(data);
        HandleMessage(message, connection);
    }
    public abstract void HandleMessage(T message, Connection connection);

    public string GetMessageTag()
    {
        return typeof(T).Name;
    }
}
