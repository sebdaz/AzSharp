using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public sealed class ValueTypeSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        switch (node.GetNodeType())
        {
            case JsonNodeType.STRING:
                return node.AsString();
            case JsonNodeType.INT:
                return node.AsInt();
            case JsonNodeType.FLOAT:
                return node.AsFloat();
            case JsonNodeType.BOOL:
                return node.AsBool();
            case JsonNodeType.NOTHING:
                return null;
            default:
                return null;
        }
    }

    public JsonNode Serialize(object obj, Type type)
    {
        if (obj == null)
        {
            return new JsonNode(JsonNodeType.NOTHING);
        }
        if (obj is bool)
        {
            return new JsonNode((bool)obj);
        }
        if (obj is string)
        {
            return new JsonNode((string)obj);
        }
        if (obj is decimal || obj is float)
        {
            return new JsonNode((float)obj);
        }
        if (obj is int || obj is uint || obj is byte || obj is Enum)
        {
            return new JsonNode((int)obj);
        }
        return new JsonNode(JsonNodeType.NOTHING);
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version) { }
}
