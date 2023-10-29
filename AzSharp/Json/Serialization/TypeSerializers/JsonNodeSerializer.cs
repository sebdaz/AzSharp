using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public sealed class JsonNodeSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        return node;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        return (JsonNode)obj;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version) { }
}

