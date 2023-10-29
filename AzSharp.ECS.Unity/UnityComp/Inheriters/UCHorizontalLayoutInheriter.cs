using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCHorizontalLayoutInheriter : UnityCompInheriter<UCHorizontalLayout>
{
    public override bool LoadFromSceneObject(UCHorizontalLayout component, GameObject gameobject)
    {
        component.HorizontalLayout = gameobject.GetComponent<HorizontalLayoutGroup>();
        if (component.HorizontalLayout == null)
        {
            return false;
        }
        return true;
    }
}
