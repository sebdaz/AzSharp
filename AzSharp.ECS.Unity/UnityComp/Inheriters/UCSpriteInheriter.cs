using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCSpriteInheriter : UnityCompInheriter<UCSprite>
{
    public override bool LoadFromSceneObject(UCSprite component, GameObject gameobject)
    {
        component.sprite = gameobject.GetComponent<SpriteRenderer>();
        if (component.sprite == null)
        {
            return false;
        }
        return true;
    }
}

