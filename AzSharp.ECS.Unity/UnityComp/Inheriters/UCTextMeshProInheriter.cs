using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCTextMeshProInheriter : UnityCompInheriter<UCTextMeshPro>
{
    public override bool LoadFromSceneObject(UCTextMeshPro component, GameObject gameobject)
    {
        component.Text = gameobject.GetComponent<TextMeshPro>();
        if (component.Text == null)
        {
            return false;
        }
        return true;
    }
}
