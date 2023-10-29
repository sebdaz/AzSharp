using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCTMPInputFieldInheriter : UnityCompInheriter<UCTMPInputField>
{
    public override bool LoadFromSceneObject(UCTMPInputField component, GameObject gameobject)
    {
        component.InputField = gameobject.GetComponent<TMP_InputField>();
        if (component.InputField == null)
        {
            return false;
        }
        return true;
    }
}
