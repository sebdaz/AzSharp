using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCCanvasInheriter : UnityCompInheriter<UCCanvas>
{
    public override bool LoadFromSceneObject(UCCanvas component, GameObject gameobject)
    {
        component.Canvas = gameobject.GetComponent<Canvas>();
        if (component.Canvas == null)
        {
            return false;
        }
        return true;
    }
}
