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

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCCanvasDataCache
{
    public RenderMode renderMode;
    public bool pixelPerfect;
    public int sortingOrder;
    public int targetDisplay;
    public AdditionalCanvasShaderChannels additionalCanvasShaderChannels;
    public UCCanvasDataCache(RenderMode renderMode, bool pixelPerfect, int sortingOrder, int targetDisplay, AdditionalCanvasShaderChannels additionalCanvasShaderChannels)
    {
        this.renderMode = renderMode;
        this.pixelPerfect = pixelPerfect;
        this.sortingOrder = sortingOrder;
        this.targetDisplay = targetDisplay;
        this.additionalCanvasShaderChannels = additionalCanvasShaderChannels;
    }
}

public sealed class UCCanvasSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        UCCanvas cast = (UCCanvas)obj;
        var dict = node.AsDict();

        RenderMode renderMode = (RenderMode)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["RenderMode"]);
        bool pixelPerfect = JsonSerializer.Deserialize<bool, ValueTypeSerializer>(null, dict["PixelPerfect"]);
        int sortingOrder = JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["SortOrder"]);
        int targetDisplay = JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["TargetDisplay"]);
        AdditionalCanvasShaderChannels additionalShaderChannels = (AdditionalCanvasShaderChannels)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["AdditionalShaderChannels"]);

        cast.dataCache = new UCCanvasDataCache(renderMode, pixelPerfect, sortingOrder, targetDisplay, additionalShaderChannels);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        UCCanvas cast = (UCCanvas)obj;
        JsonSerializer.AssertObject(cast.Canvas);
        Canvas canvas = cast.Canvas;

        dict["RenderMode"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)canvas.renderMode);
        dict["PixelPerfect"] = JsonSerializer.Serialize<bool, ValueTypeSerializer>(canvas.pixelPerfect);
        dict["SortOrder"] = JsonSerializer.Serialize<int, ValueTypeSerializer>(canvas.sortingOrder);
        dict["TargetDisplay"] = JsonSerializer.Serialize<int, ValueTypeSerializer>(canvas.targetDisplay);
        dict["AdditionalShaderChannels"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)canvas.additionalShaderChannels);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}

