using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCStandaloneInputModuleInheriter : UnityCompInheriter<UCStandaloneInputModule>
{
    public override bool LoadFromSceneObject(UCStandaloneInputModule component, GameObject gameobject)
    {
        component.InputModule = gameobject.GetComponent<StandaloneInputModule>();
        if (component.InputModule == null)
        {
            return false;
        }
        return true;
    }
}
