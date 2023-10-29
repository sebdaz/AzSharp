using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCEventSystemInheriter : UnityCompInheriter<UCEventSystem>
{
    public override bool LoadFromSceneObject(UCEventSystem component, GameObject gameobject)
    {
        component.EventSystem = gameobject.GetComponent<EventSystem>();
        if (component.EventSystem == null)
        {
            return false;
        }
        return true;
    }
}
