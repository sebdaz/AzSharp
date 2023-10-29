using System;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.Json.Serialization;
using AzSharp.ECS.Unity.Json.TypeSerializers;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.ECS.Shared.Components;
using AzSharp.IoC;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCCanvasScalerDataCache
{
    public CanvasScaler.ScaleMode scaleMode;
    public Vector2 referenceResolution;
    public CanvasScaler.ScreenMatchMode screenMatchMode;
    public float matchWidthOrHeight;
    public UCCanvasScalerDataCache(CanvasScaler.ScaleMode scaleMode, Vector2 referenceResolution, CanvasScaler.ScreenMatchMode screenMatchMode, float matchWidthOrHeight)
    {
        this.scaleMode = scaleMode;
        this.referenceResolution = referenceResolution;
        this.screenMatchMode = screenMatchMode;
        this.matchWidthOrHeight = matchWidthOrHeight;
    }
}

public sealed class UCCanvasScalerSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        UCCanvasScaler cast = (UCCanvasScaler)obj;
        var dict = node.AsDict();

        CanvasScaler.ScaleMode uiScaleMode = (CanvasScaler.ScaleMode)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["UIScaleMode"]);
        Vector2 referenceResolution = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, dict["ReferenceResolution"]);
        CanvasScaler.ScreenMatchMode screenMatchMode = (CanvasScaler.ScreenMatchMode)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["ScreenMatchMode"]);
        float matchWidthOrHeight = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["Match"]);

        cast.dataCache = new UCCanvasScalerDataCache(uiScaleMode, referenceResolution, screenMatchMode, matchWidthOrHeight);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        UCCanvasScaler cast = (UCCanvasScaler)obj;
        JsonSerializer.AssertObject(cast.CanvasScaler);
        CanvasScaler scaler = cast.CanvasScaler;
        var dict = node.AsDict();

        dict["UIScaleMode"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)scaler.uiScaleMode);
        dict["ReferenceResolution"] = JsonSerializer.Serialize<Vector2, Vector2Serializer>(scaler.referenceResolution);
        dict["ScreenMatchMode"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)scaler.screenMatchMode);
        dict["Match"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(scaler.matchWidthOrHeight);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}

