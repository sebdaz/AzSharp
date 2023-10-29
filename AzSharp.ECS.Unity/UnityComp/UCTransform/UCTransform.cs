using System;
using System.Collections.Generic;
using System.Text;
using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Entities;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCTransformSerializer))]
[RegisterComponent(typeof(ComponentArray<UCTransform>), typeof(ComponentEventRaiser<UCTransform>), 10000)]
public sealed class UCTransform
{
    public GameObject? gameObject = null;
    public UCTransformDataCache? dataCache = null;
    public GameObject GameObject
    {
        get
        {
            if (gameObject == null)
            {
                throw new InvalidOperationException("Game Object of UCTransform is null");
            }
            return gameObject;
        }
    }
    public RectTransform RectTransform
    {
        get
        {
            GameObject gameObject = GameObject;
            return gameObject.GetComponent<RectTransform>();
        }
    }
    public UCTransform() { }
}
