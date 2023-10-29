using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.EventSystems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCEventSystemSerializer))]
[RegisterComponent(typeof(ComponentArray<UCEventSystem>), typeof(ComponentEventRaiser<UCEventSystem>), 100)]
public sealed class UCEventSystem
{
    public EventSystem? EventSystem = null;
}
