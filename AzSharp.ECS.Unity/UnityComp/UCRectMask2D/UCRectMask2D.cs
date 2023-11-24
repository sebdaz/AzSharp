using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.Json.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;


[JsonSerializable(typeof(UCRectMask2DSerializer))]
[RegisterComponent(typeof(ComponentArray<UCRectMask2D>), typeof(ComponentEventRaiser<UCRectMask2D>))]
public sealed class UCRectMask2D
{
    public RectMask2D? rectMask2D = null;
}
