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

public sealed class UCCameraDataCache
{
    public CameraClearFlags clearFlags;
    public Color backgroundColor;
    public int cullingMask;
    public Matrix4x4 projectionMatrix;
    public float fieldOfView;
    public float nearClipPlane;
    public float farClipPlane;
    public UCCameraDataCache(CameraClearFlags clearFlags, Color backgroundColor, int cullingMask, Matrix4x4 projectionMatrix, float fieldOfView, float nearClipPlane, float farClipPlane)
    {
        this.clearFlags = clearFlags;
        this.backgroundColor = backgroundColor;
        this.cullingMask = cullingMask;
        this.projectionMatrix = projectionMatrix;
        this.fieldOfView = fieldOfView;
        this.nearClipPlane = nearClipPlane;
        this.farClipPlane = farClipPlane;
    }
}

public sealed class UCCameraSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        UCCamera cast = (UCCamera)obj;
        var dict = node.AsDict();

        CameraClearFlags clearFlags = (CameraClearFlags)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["ClearFlags"]);
        Color backgroundColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["Background"]);
        int cullingMask = JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["CullingMask"]);
        Matrix4x4 projectionMatrix = JsonSerializer.Deserialize<Matrix4x4, Matrix4x4Serializer>(null, dict["Projection"]);
        float fieldOfView = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["FOV"]);
        float nearClipPlane = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["NearClip"]);
        float farClipPlane = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["FarClip"]); ;

        cast.dataCache = new UCCameraDataCache(clearFlags, backgroundColor, cullingMask, projectionMatrix, fieldOfView, nearClipPlane, farClipPlane);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        UCCamera cast = (UCCamera)obj;
        JsonSerializer.AssertObject(cast.Camera);
        Camera camera = cast.Camera;

        dict["ClearFlags"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)camera.clearFlags);
        dict["Background"] = JsonSerializer.Serialize<Color, ColorSerializer>(camera.backgroundColor);
        dict["CullingMask"] = JsonSerializer.Serialize<int, ValueTypeSerializer>(camera.cullingMask);
        dict["Projection"] = JsonSerializer.Serialize<Matrix4x4, Matrix4x4Serializer>(camera.projectionMatrix);
        dict["FOV"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(camera.fieldOfView);
        dict["NearClip"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(camera.nearClipPlane);
        dict["FarClip"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(camera.farClipPlane);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
