using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCButtonSerializer))]
[RegisterComponent(typeof(ComponentArray<UCButton>), typeof(ComponentEventRaiser<UCButton>))]
public sealed class UCButton
{
    public Button? Button = null;
}
