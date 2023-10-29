using Network;
using Network.Enums;
using Network.Interfaces;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

public interface INetHandlerManager
{
    public void RegisterNetConnectionHandler(Type handler_type, NetHandlerTag tag);
    public void RegisterNetMessageHandler(Type handler_type, NetHandlerTag tag);
    public void RegisterFromAttributes();
    public void HandleNetMessage(Connection connection, string message_tag, byte[] data, List<NetHandlerTag> tags);
    public void HandleNetConnectionEstablish(Connection connection, List<NetHandlerTag> tags, ConnectionType type);
    public void HandleNetConnectionLost(Connection connection, List<NetHandlerTag> tags, ConnectionType type, CloseReason reason);
    public void DoConnectionRegistrations(Connection connection, List<NetHandlerTag> tags, PacketReceivedHandler<RawData> callback);
    public void DoConnectionUnregistrations(Connection connection, List<NetHandlerTag> tags);
}
