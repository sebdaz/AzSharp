using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;
using System;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCCameraSerializer))]
[RegisterComponent(typeof(ComponentArray<UCCamera>), typeof(ComponentEventRaiser<UCCamera>), 200)]
public sealed class UCCamera
{
    public Camera? camera = null;
    public UCCameraDataCache? dataCache = null;
    public Camera Camera
    {
        get
        {
            if (camera == null)
            {
                throw new InvalidOperationException("Camera of UCCamera is null");
            }
            return camera;
        }
    }
}
