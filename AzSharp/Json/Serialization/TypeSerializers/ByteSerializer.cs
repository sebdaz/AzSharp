using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public sealed class ByteSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        return Convert.ToByte(node.AsInt());
    }

    public JsonNode Serialize(object obj, Type type)
    {
        return new JsonNode(Convert.ToInt32((byte)obj));
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
