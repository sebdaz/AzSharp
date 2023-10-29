using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCMeshFilterSerializer))]
[RegisterComponent(typeof(ComponentArray<UCMeshFilter>), typeof(ComponentEventRaiser<UCMeshFilter>))]
public sealed class UCMeshFilter
{
    public MeshFilter? MeshFilter = null;
}
