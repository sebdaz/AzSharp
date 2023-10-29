using System;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.ECS.Shared.Components;
using AzSharp.IoC;
using AzSharp.Json.Serialization;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCGraphicRaycasterSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCGraphicRaycaster cast = (UCGraphicRaycaster)obj;
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
