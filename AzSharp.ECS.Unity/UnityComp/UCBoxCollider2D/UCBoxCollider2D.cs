using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCBoxCollider2DSerializer))]
[RegisterComponent(typeof(ComponentArray<UCBoxCollider2D>), typeof(ComponentEventRaiser<UCBoxCollider2D>))]
public sealed class UCBoxCollider2D
{
    public BoxCollider2D? BoxCollider = null;
}
