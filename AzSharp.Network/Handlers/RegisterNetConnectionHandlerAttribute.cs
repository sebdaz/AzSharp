using Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterNetConnectionHandlerAttribute : Attribute
{
    public NetHandlerTag networkHandlerTag;
    public RegisterNetConnectionHandlerAttribute(ConnectionType connection_type, string tag)
    {
        this.networkHandlerTag = new NetHandlerTag(connection_type, tag);
    }
}
