using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCCanvasScalerSerializer))]
[RegisterComponent(typeof(ComponentArray<UCCanvasScaler>), typeof(ComponentEventRaiser<UCCanvasScaler>), 49)]
public sealed class UCCanvasScaler
{
    public CanvasScaler? CanvasScaler = null;
    public UCCanvasScalerDataCache? dataCache = null;
}
