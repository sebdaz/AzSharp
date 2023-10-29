using AzSharp.IoC;
using AzSharp.Prototype;
using AzSharp.Reflection;
using Network;
using Network.Enums;
using Network.Interfaces;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

public sealed class NetHandlerManager : INetHandlerManager
{
    private Dictionary<NetHandlerTag, List<INetMessageHandler>> messageHandlersMap = new();
    private Dictionary<NetHandlerTag, List<INetConnectionHandler>> connectionHandlersMap = new();
    private List<INetMessageHandler> GetMessageHandlerList(NetHandlerTag tag)
    {
        if (!messageHandlersMap.ContainsKey(tag))
        {
            messageHandlersMap[tag] = new List<INetMessageHandler>();
        }
        return messageHandlersMap[tag];
    }
    private List<INetConnectionHandler> GetConnectionHandlerList(NetHandlerTag tag)
    {
        if (!connectionHandlersMap.ContainsKey(tag))
        {
            connectionHandlersMap[tag] = new List<INetConnectionHandler>();
        }
        return connectionHandlersMap[tag];
    }

    public void HandleNetConnectionEstablish(Connection connection, List<NetHandlerTag> tags, ConnectionType type)
    {
        foreach (var tag in tags)
        {
            if (tag.connectionType != type)
            {
                continue;
            }
            foreach (var handler in GetConnectionHandlerList(tag))
            {
                handler.OnConnectionEstablished(connection);
            }
        }
    }

    public void HandleNetConnectionLost(Connection connection, List<NetHandlerTag> tags, ConnectionType type, CloseReason reason)
    {
        foreach (var tag in tags)
        {
            if (tag.connectionType != type)
            {
                continue;
            }
            foreach (var handler in GetConnectionHandlerList(tag))
            {
                handler.OnConnectionLost(connection, reason);
            }
        }
    }

    public void HandleNetMessage(Connection connection, string message_tag, byte[] data, List<NetHandlerTag> tags)
    {
        foreach (var tag in tags)
        {
            foreach (var handler in GetMessageHandlerList(tag))
            {
                if (handler.GetMessageTag() != message_tag)
                {
                    continue;
                }
                handler.ReceiveMessage(data, connection);
            }
        }
    }

    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterNetConnectionHandlerAttribute>())
        {
            RegisterNetConnectionHandlerAttribute attribute = (RegisterNetConnectionHandlerAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterNetConnectionHandlerAttribute));
            RegisterNetConnectionHandler(type, attribute.networkHandlerTag);
        }
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterNetMessageHandlerAttribute>())
        {
            RegisterNetMessageHandlerAttribute attribute = (RegisterNetMessageHandlerAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterNetMessageHandlerAttribute));
            RegisterNetMessageHandler(type, attribute.networkHandlerTag);
        }
    }

    public void RegisterNetConnectionHandler(Type handler_type, NetHandlerTag tag)
    {
        INetConnectionHandler handler = (INetConnectionHandler)Activator.CreateInstance(handler_type);
        var list = GetConnectionHandlerList(tag);
        list.Add(handler);
    }

    public void RegisterNetMessageHandler(Type handler_type, NetHandlerTag tag)
    {
        INetMessageHandler handler = (INetMessageHandler)Activator.CreateInstance(handler_type);
        var list = GetMessageHandlerList(tag);
        list.Add(handler);
    }
    public void DoConnectionRegistrations(Connection connection, List<NetHandlerTag> tags, PacketReceivedHandler<RawData> callback)
    {
        foreach (var tag in tags)
        {
            foreach (var message_handler in GetMessageHandlerList(tag))
            {
                connection.RegisterRawDataHandler(message_handler.GetMessageTag(), callback);
            }
        }
    }

    public void DoConnectionUnregistrations(Connection connection, List<NetHandlerTag> tags)
    {
        foreach (var tag in tags)
        {
            foreach (var message_handler in GetMessageHandlerList(tag))
            {
                connection.UnRegisterRawDataHandler(message_handler.GetMessageTag());
            }
        }
    }
}
