using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCLightInheriter : UnityCompInheriter<UCLight>
{
    public override bool LoadFromSceneObject(UCLight component, GameObject gameobject)
    {
        component.Light = gameobject.GetComponent<Light>();
        if (component.Light == null)
        {
            return false;
        }
        return true;
    }
}
