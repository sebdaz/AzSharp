using Network.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Network.Handlers;

public struct NetHandlerTag
{
    public ConnectionType connectionType;
    public string tag;
    public NetHandlerTag(ConnectionType connectionType, string tag)
    {
        this.connectionType = connectionType;
        this.tag = tag;
    }
}
