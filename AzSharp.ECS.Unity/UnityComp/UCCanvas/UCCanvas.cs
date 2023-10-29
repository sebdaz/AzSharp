using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCCanvasSerializer))]
[RegisterComponent(typeof(ComponentArray<UCCanvas>), typeof(ComponentEventRaiser<UCCanvas>), 50)]
public sealed class UCCanvas
{
    public Canvas? Canvas = null;
    public UCCanvasDataCache? dataCache = null;
}
