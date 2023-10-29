using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCSliderSerializer))]
[RegisterComponent(typeof(ComponentArray<UCSlider>), typeof(ComponentEventRaiser<UCSlider>))]
public sealed class UCSlider
{
    public Slider? Slider = null;
}
