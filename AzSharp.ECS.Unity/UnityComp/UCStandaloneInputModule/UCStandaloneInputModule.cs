using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.EventSystems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCStandaloneInputModuleSerializer))]
[RegisterComponent(typeof(ComponentArray<UCStandaloneInputModule>), typeof(ComponentEventRaiser<UCStandaloneInputModule>))]
public sealed class UCStandaloneInputModule
{
    public StandaloneInputModule? InputModule = null;
}
