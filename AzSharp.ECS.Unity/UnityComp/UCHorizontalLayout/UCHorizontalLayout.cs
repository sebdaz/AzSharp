using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCHorizontalLayoutSerializer))]
[RegisterComponent(typeof(ComponentArray<UCHorizontalLayout>), typeof(ComponentEventRaiser<UCHorizontalLayout>))]
public sealed class UCHorizontalLayout
{
    public HorizontalLayoutGroup? HorizontalLayout = null;
}
