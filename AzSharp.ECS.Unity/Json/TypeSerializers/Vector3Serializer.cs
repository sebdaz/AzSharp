using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.Json.TypeSerializers;

public sealed class Vector3Serializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var node_List = node.AsList();
        return new Vector3(node_List[0].AsFloat(), node_List[1].AsFloat(), node_List[2].AsFloat());
    }

    public JsonNode Serialize(object obj, Type type)
    {
        Vector3 cast = (Vector3)obj;
        JsonNode list_node = new JsonNode(JsonNodeType.LIST);
        var list = list_node.AsList();
        list.Add(new JsonNode(cast.x));
        list.Add(new JsonNode(cast.y));
        list.Add(new JsonNode(cast.z));
        return list_node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
