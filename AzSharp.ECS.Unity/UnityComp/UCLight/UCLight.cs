using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCLightSerializer))]
[RegisterComponent(typeof(ComponentArray<UCLight>), typeof(ComponentEventRaiser<UCLight>))]
public sealed class UCLight
{
    public Light? Light = null;
    public UCLightDataCache? dataCache = null;
}
