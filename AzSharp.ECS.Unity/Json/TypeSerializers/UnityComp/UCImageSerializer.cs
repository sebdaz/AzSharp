using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.Json.TypeSerializers;
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

public sealed class UCImageDataCache
{
    public Color color;
    public UCImageDataCache(Color color)
    {
        this.color = color;
    }
}

public sealed class UCImageSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        UCImage cast = (UCImage)obj;

        var dict = node.AsDict();

        Color color = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["Color"]);
        cast.dataCache = new UCImageDataCache(color);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();

        UCImage cast = (UCImage)obj;
        JsonSerializer.AssertObject(cast.image);
        dict["Color"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.image.color);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
