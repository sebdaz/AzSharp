using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;


public sealed class UCRectMask2DInheriter : UnityCompInheriter<UCRectMask2D>
{
    public override bool LoadFromSceneObject(UCRectMask2D component, GameObject gameobject)
    {
        component.rectMask2D = gameobject.GetComponent<RectMask2D>();
        if (component.rectMask2D == null)
        {
            return false;
        }
        return true;
    }
}
