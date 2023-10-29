using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCCanvasScalerInheriter : UnityCompInheriter<UCCanvasScaler>
{
    public override bool LoadFromSceneObject(UCCanvasScaler component, GameObject gameobject)
    {
        component.CanvasScaler = gameobject.GetComponent<CanvasScaler>();
        if (component.CanvasScaler == null)
        {
            return false;
        }
        return true;
    }
}
