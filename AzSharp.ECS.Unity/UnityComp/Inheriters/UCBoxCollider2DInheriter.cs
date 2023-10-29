using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCBoxCollider2DInheriter : UnityCompInheriter<UCBoxCollider2D>
{
    public override bool LoadFromSceneObject(UCBoxCollider2D component, GameObject gameobject)
    {
        component.BoxCollider = gameobject.GetComponent<BoxCollider2D>();
        if (component.BoxCollider == null)
        {
            return false;
        }
        return true;
    }
}
