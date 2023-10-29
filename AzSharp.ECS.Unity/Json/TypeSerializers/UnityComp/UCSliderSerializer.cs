
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.IoC;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCSliderSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCSlider cast = (UCSlider)obj;
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
