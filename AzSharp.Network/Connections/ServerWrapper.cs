using AzSharp.IoC;
using AzSharp.Network.Handlers;
using MessagePack;
using Network;
using Network.Enums;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace AzSharp.Network.Connections;

public sealed class ServerWrapper
{
    public int port = 0;
    public ServerConnectionContainer? connectionContainer;
    public List<Connection> connectionsUDP = new();
    public List<Connection> connectionsTCP = new();
    private INetHandlerManager net_manager;
    private List<NetHandlerTag> tags = new();
    public ServerWrapper()
    {
        net_manager = IoCManager.Resolve<INetHandlerManager>();
    }
    public void MessageBroadcastTCP<T>(T message)
    {
        string type_name = typeof(T).Name;
        byte[] bytes = MessagePackSerializer.Serialize(message);
        foreach (var connection in connectionsTCP)
        {
            connection.SendRawData(type_name, bytes);
        }
    }
    public void MessageBroadcastUDP<T>(T message)
    {
        string type_name = typeof(T).Name;
        byte[] bytes = MessagePackSerializer.Serialize(message);
        foreach (var connection in connectionsTCP)
        {
            connection.SendRawData(type_name, bytes);
        }
    }
    public void SpecificMessageBroadcast<T>(T message, List<Connection> specific_connections)
    {
        string type_name = typeof(T).Name;
        byte[] bytes = MessagePackSerializer.Serialize(message);
        foreach (var connection in specific_connections)
        {
            connection.SendRawData(type_name, bytes);
        }
    }
    public void AddTag(NetHandlerTag tag)
    {
        tags.Add(tag);
    }
    public void Setup(bool secure = false)
    {
        if (connectionContainer != null)
        {
            return;
        }
        if (secure)
        {
            connectionContainer = ConnectionFactory.CreateSecureServerConnectionContainer(port, 2048, false);
        }
        else
        {
            connectionContainer = ConnectionFactory.CreateServerConnectionContainer(port, false);
        }
        connectionContainer.ConnectionEstablished += OnConnectionEstablished;
        connectionContainer.ConnectionLost += OnConnectionLost;
    }
    public void Start()
    {
        if (connectionContainer == null)
        {
            throw new InvalidOperationException("Tried to start a server without Setup()'ing it first");
        }
        connectionContainer.Start();
    }
    public void Stop()
    {
        if (connectionContainer == null)
        {
            return;
        }
        connectionContainer.Stop();
    }
    private void OnConnectionEstablished(Connection connection, ConnectionType type)
    {
        switch (type)
        {
            case ConnectionType.TCP:
                connectionsTCP.Add(connection);
                break;
            case ConnectionType.UDP:
                connectionsUDP.Add(connection);
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
                connectionsTCP.Remove(connection);
                break;
            case ConnectionType.UDP:
                connectionsUDP.Remove(connection);
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
