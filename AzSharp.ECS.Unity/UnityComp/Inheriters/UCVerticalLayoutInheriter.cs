using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCVerticalLayoutInheriter : UnityCompInheriter<UCVerticalLayout>
{
    public override bool LoadFromSceneObject(UCVerticalLayout component, GameObject gameobject)
    {
        component.VerticalLayout = gameobject.GetComponent<VerticalLayoutGroup>();
        if (component.VerticalLayout == null)
        {
            return false;
        }
        return true;
    }
}
