using Network;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

internal interface INetMessageHandler
{
    public void ReceiveMessage(byte[] data, Connection connection);
    public string GetMessageTag();
}
