using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.EventSystems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCPhysics2DRaycasterSerializer))]
[RegisterComponent(typeof(ComponentArray<UCPhysics2DRaycaster>), typeof(ComponentEventRaiser<UCPhysics2DRaycaster>))]
public sealed class UCPhysics2DRaycaster
{
    public Physics2DRaycaster? Physics2DRaycaster = null;
}
