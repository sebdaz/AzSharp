using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.Json.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[JsonSerializable(typeof(UCSpriteSerializer))]
[RegisterComponent(typeof(ComponentArray<UCSprite>), typeof(ComponentEventRaiser<UCSprite>))]
public sealed class UCSprite
{
    public SpriteRenderer? sprite = null;
    public UCSpriteDataCache? cache = null;
    public SpriteRenderer Sprite
    {
        get
        {
            if (sprite == null)
            {
                throw new InvalidOperationException("Sprite of UCSprite is null");
            }
            return sprite;
        }
    }
}
