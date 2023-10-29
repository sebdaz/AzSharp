using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCCameraInheriter : UnityCompInheriter<UCCamera>
{
    public override bool LoadFromSceneObject(UCCamera component, GameObject gameobject)
    {
        component.Camera = gameobject.GetComponent<Camera>();
        if (component.Camera == null)
        {
            return false;
        }
        return true;
    }
}
