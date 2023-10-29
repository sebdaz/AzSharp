using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AzSharp.ECS.Unity.Mono;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCMouseEventsInheriter : UnityCompInheriter<UCMouseEvents>
{
    public override bool LoadFromSceneObject(UCMouseEvents component, GameObject gameobject)
    {
        component.MouseEvents = gameobject.GetComponent<EntityMouseEvents>();
        if (component.MouseEvents == null)
        {
            return false;
        }
        return true;
    }
}
