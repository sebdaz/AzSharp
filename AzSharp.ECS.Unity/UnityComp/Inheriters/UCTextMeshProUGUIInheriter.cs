using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCTextMeshProUGUIInheriter : UnityCompInheriter<UCTextMeshProUGUI>
{
    public override bool LoadFromSceneObject(UCTextMeshProUGUI component, GameObject gameobject)
    {
        component.text = gameobject.GetComponent<TextMeshProUGUI>();
        if (component.text == null)
        {
            return false;
        }
        return true;
    }
}
