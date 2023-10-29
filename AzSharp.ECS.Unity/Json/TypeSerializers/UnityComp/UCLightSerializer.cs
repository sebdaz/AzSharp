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

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCLightDataCache
{
    public LightType lightType;
    public LightShadowCasterMode lightShadowCasterMode;
    public Color color;
    public float intensity;
    public int cullingMask;
    public UCLightDataCache(LightType lightType, LightShadowCasterMode lightShadowCasterMode, Color color, float intensity, int cullingMask)
    {
        this.lightType = lightType;
        this.lightShadowCasterMode = lightShadowCasterMode;
        this.color = color;
        this.intensity = intensity;
        this.cullingMask = cullingMask;
    }
}

public sealed class UCLightSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        UCLight cast = (UCLight)obj;

        var dict = node.AsDict();

        LightType lightType = (LightType)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["Type"]);
        LightShadowCasterMode lightShadowCasterMode = (LightShadowCasterMode)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["Mode"]);
        Color color = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["Color"]);
        float intensity = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["Intensity"]);
        LightRenderMode renderMode = (LightRenderMode)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["RenderMode"]);
        int cullingMask = JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["CullingMask"]);

        cast.dataCache = new UCLightDataCache(lightType, lightShadowCasterMode, color, intensity, cullingMask);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();

        UCLight cast = (UCLight)obj;
        JsonSerializer.AssertObject(cast.Light);
        Light light = cast.Light;

        dict["Type"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)light.type);
        dict["Mode"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)light.lightShadowCasterMode);
        dict["Color"] = JsonSerializer.Serialize<Color, ColorSerializer>(light.color);
        dict["Intensity"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(light.intensity);
        dict["RenderMode"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)light.renderMode);
        dict["CullingMask"] = JsonSerializer.Serialize<int, ValueTypeSerializer>(light.cullingMask);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
