using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.Json.TypeSerializers;

public sealed class Matrix4x4Serializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var node_list = node.AsList();
        Matrix4x4 matrix = new();
        matrix.SetRow(0, JsonSerializer.Deserialize<Vector4, Vector4Serializer>(null, node_list[0]));
        matrix.SetRow(1, JsonSerializer.Deserialize<Vector4, Vector4Serializer>(null, node_list[1]));
        matrix.SetRow(2, JsonSerializer.Deserialize<Vector4, Vector4Serializer>(null, node_list[2]));
        matrix.SetRow(3, JsonSerializer.Deserialize<Vector4, Vector4Serializer>(null, node_list[3]));
        return matrix;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        Matrix4x4 cast = (Matrix4x4)obj;
        JsonNode list_node = new JsonNode(JsonNodeType.LIST);
        var list = list_node.AsList();
        list.Add(JsonSerializer.Serialize<Vector4, Vector4Serializer>(cast.GetRow(0)));
        list.Add(JsonSerializer.Serialize<Vector4, Vector4Serializer>(cast.GetRow(1)));
        list.Add(JsonSerializer.Serialize<Vector4, Vector4Serializer>(cast.GetRow(2)));
        list.Add(JsonSerializer.Serialize<Vector4, Vector4Serializer>(cast.GetRow(3)));
        return list_node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
