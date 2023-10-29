using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using TMPro;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCTextMeshProSerializer))]
[RegisterComponent(typeof(ComponentArray<UCTextMeshPro>), typeof(ComponentEventRaiser<UCTextMeshPro>))]
public sealed class UCTextMeshPro
{
    public TextMeshPro? Text = null;
    public UCTextMeshProDataCache? dataCache = null;
}
