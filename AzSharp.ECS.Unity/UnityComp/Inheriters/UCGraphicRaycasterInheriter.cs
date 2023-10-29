using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCGraphicRaycasterInheriter : UnityCompInheriter<UCGraphicRaycaster>
{
    public override bool LoadFromSceneObject(UCGraphicRaycaster component, GameObject gameobject)
    {
        component.GraphicRaycaster = gameobject.GetComponent<GraphicRaycaster>();
        if (component.GraphicRaycaster == null)
        {
            return false;
        }
        return true;
    }
}
