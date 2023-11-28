using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using TMPro;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;
using System;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCTextMeshProUGUISerializer))]
[RegisterComponent(typeof(ComponentArray<UCTextMeshProUGUI>), typeof(ComponentEventRaiser<UCTextMeshProUGUI>))]
public sealed class UCTextMeshProUGUI
{
    public TextMeshProUGUI? text = null;
    public UCTextMeshProDataCache? dataCache = null;
    public TextMeshProUGUI Text
    {
        get
        {
            if (text == null)
            {
                throw new InvalidOperationException("Text of UCTextMeshProUGUI is null");
            }
            return text;
        }
    }
}
