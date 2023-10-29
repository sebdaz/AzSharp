using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using TMPro;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCTMPInputFieldSerializer))]
[RegisterComponent(typeof(ComponentArray<UCTMPInputField>), typeof(ComponentEventRaiser<UCTMPInputField>))]
public sealed class UCTMPInputField
{
    public TMP_InputField? InputField = null;
}
