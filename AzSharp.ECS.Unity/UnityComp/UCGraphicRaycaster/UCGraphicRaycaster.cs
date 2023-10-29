using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCGraphicRaycasterSerializer))]
[RegisterComponent(typeof(ComponentArray<UCGraphicRaycaster>), typeof(ComponentEventRaiser<UCGraphicRaycaster>))]
public sealed class UCGraphicRaycaster
{
    public GraphicRaycaster? GraphicRaycaster = null;
}
