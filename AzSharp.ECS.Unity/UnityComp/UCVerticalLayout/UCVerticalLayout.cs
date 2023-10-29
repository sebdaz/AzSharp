using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCVerticalLayoutSerializer))]
[RegisterComponent(typeof(ComponentArray<UCVerticalLayout>), typeof(ComponentEventRaiser<UCVerticalLayout>))]
public sealed class UCVerticalLayout
{
    public VerticalLayoutGroup? VerticalLayout = null;
}
