using AzSharp.IoC;
using AzSharp.Network.Handlers;
using Network;
using Network.Enums;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Connections;

public sealed class ClientWrapper
{
    public string ip = string.Empty;
    public int port = 0;
    public ClientConnectionContainer? connectionContainer;
    public Connection? connectionUDP = null;
    public Connection? connectionTCP = null;
    private INetHandlerManager net_manager;
    private List<NetHandlerTag> tags = new();
    public ClientWrapper()
    {
        net_manager = IoCManager.Resolve<INetHandlerManager>();
    }
    public void AddTag(NetHandlerTag tag)
    {
        tags.Add(tag);
    }
    public void Start(bool secure = false)
    {
        if (connectionContainer != null)
        {
            connectionContainer.Reconnect();
            return;
        }
        if (secure)
        {
            connectionContainer = ConnectionFactory.CreateSecureClientConnectionContainer(ip, port);
        }
        else
        {
            connectionContainer = ConnectionFactory.CreateClientConnectionContainer(ip, port);
        }
        connectionContainer.ConnectionEstablished += OnConnectionEstablished;
        connectionContainer.ConnectionLost += OnConnectionLost;
    }
    public void Stop()
    {
        if (connectionContainer == null)
        {
            return;
        }
        connectionContainer.Shutdown(CloseReason.ClientClosed, true);
    }
    public bool IsConnectedTCP()
    {
        if (connectionContainer == null)
        {
            return false;
        }
        return connectionContainer.IsAlive_TCP;
    }
    public bool IsConnectedUDP()
    {
        if (connectionContainer == null)
        {
            return false;
        }
        return connectionContainer.IsAlive_UDP;
    }
    private void OnConnectionEstablished(Connection connection, ConnectionType type)
    {
        switch (type)
        {
            case ConnectionType.TCP:
                connectionTCP = connection;
                break;
            case ConnectionType.UDP:
                connectionUDP = connection;
                break;
            default:
                break;
        }
        net_manager.DoConnectionRegistrations(connection, tags, OnRawData);
        net_manager.HandleNetConnectionEstablish(connection, tags, type);
    }
    private void OnConnectionLost(Connection connection, ConnectionType type, CloseReason reason)
    {
        switch (type)
        {
            case ConnectionType.TCP:
                connectionTCP = null;
                break;
            case ConnectionType.UDP:
                connectionUDP = null;
                break;
            default:
                break;
        }
        net_manager.DoConnectionUnregistrations(connection, tags);
        net_manager.HandleNetConnectionLost(connection, tags, type, reason);
    }

    private void OnRawData(RawData packet, Connection connection)
    {
        net_manager.HandleNetMessage(connection, packet.Key, packet.Data, tags);
    }
}
