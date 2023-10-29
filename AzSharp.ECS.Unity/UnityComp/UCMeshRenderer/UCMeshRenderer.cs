using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCMeshRendererSerializer))]
[RegisterComponent(typeof(ComponentArray<UCMeshRenderer>), typeof(ComponentEventRaiser<UCMeshRenderer>), 300)]
public sealed class UCMeshRenderer
{
    public MeshRenderer? MeshRenderer = null;
}
