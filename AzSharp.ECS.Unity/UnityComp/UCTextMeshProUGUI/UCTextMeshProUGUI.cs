using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using TMPro;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCTextMeshProUGUISerializer))]
[RegisterComponent(typeof(ComponentArray<UCTextMeshProUGUI>), typeof(ComponentEventRaiser<UCTextMeshProUGUI>))]
public sealed class UCTextMeshProUGUI
{
    public TextMeshProUGUI? Text = null;
    public UCTextMeshProDataCache? dataCache = null;
}
