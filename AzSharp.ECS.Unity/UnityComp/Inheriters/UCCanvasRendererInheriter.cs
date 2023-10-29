using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCCanvasRendererInheriter : UnityCompInheriter<UCCanvasRenderer>
{
    public override bool LoadFromSceneObject(UCCanvasRenderer component, GameObject gameobject)
    {
        component.CanvasRenderer = gameobject.GetComponent<CanvasRenderer>();
        if (component.CanvasRenderer == null)
        {
            return false;
        }
        return true;
    }
}
