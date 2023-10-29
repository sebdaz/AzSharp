using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCAudioListenerSerializer))]
[RegisterComponent(typeof(ComponentArray<UCAudioListener>), typeof(ComponentEventRaiser<UCAudioListener>))]
public sealed class UCAudioListener
{
    public AudioListener? AudioListener = null;

}
