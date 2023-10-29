using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCTransformInheriter : UnityCompInheriter<UCTransform>
{
    public override bool LoadFromSceneObject(UCTransform component, GameObject gameobject)
    {
        component.gameObject = gameobject;
        return true;
    }
}
