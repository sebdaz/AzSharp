using AzSharp.ECS.Unity.UnityComp;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

internal class UCRectMask2DSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCRectMask2D cast = (UCRectMask2D)obj;
        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}