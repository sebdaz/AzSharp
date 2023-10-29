using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.ECS.Shared.Components;
using System;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCImageSerializer))]
[RegisterComponent(typeof(ComponentArray<UCImage>), typeof(ComponentEventRaiser<UCImage>))]
public sealed class UCImage
{
    public Image? image = null;
    public UCImageDataCache? dataCache = null;
    public Image Image
    {
        get
        {
            if (image == null)
            {
                throw new InvalidOperationException("Image is null");
            }
            return image;
        }
    }
}
