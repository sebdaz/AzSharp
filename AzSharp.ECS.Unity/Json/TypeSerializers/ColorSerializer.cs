using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.Json.TypeSerializers;

public sealed class ColorSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var node_List = node.AsList();
        return new Color(node_List[0].AsFloat(), node_List[1].AsFloat(), node_List[2].AsFloat(), node_List[3].AsFloat());
    }

    public JsonNode Serialize(object obj, Type type)
    {
        Color cast = (Color)obj;
        JsonNode list_node = new JsonNode(JsonNodeType.LIST);
        var list = list_node.AsList();
        list.Add(new JsonNode(cast.r));
        list.Add(new JsonNode(cast.g));
        list.Add(new JsonNode(cast.b));
        list.Add(new JsonNode(cast.a));
        return list_node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
