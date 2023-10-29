using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Unity.Mono;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCMouseEventsSerializer))]
[RegisterComponent(typeof(ComponentArray<UCMouseEvents>), typeof(ComponentEventRaiser<UCMouseEvents>))]
public sealed class UCMouseEvents
{
    public EntityMouseEvents? MouseEvents = null;
}
