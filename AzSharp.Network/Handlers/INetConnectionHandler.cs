using Network;
using Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

public interface INetConnectionHandler
{
    public void OnConnectionEstablished(Connection connection);
    public void OnConnectionLost(Connection connection, CloseReason reason);
}
