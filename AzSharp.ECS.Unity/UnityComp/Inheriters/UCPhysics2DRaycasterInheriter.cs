using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCPhysics2DRaycasterInheriter : UnityCompInheriter<UCPhysics2DRaycaster>
{
    public override bool LoadFromSceneObject(UCPhysics2DRaycaster component, GameObject gameobject)
    {
        component.Physics2DRaycaster = gameobject.GetComponent<Physics2DRaycaster>();
        if (component.Physics2DRaycaster == null)
        {
            return false;
        }
        return true;
    }
}
