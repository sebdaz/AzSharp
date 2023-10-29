using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCButtonInheriter : UnityCompInheriter<UCButton>
{
    public override bool LoadFromSceneObject(UCButton component, GameObject gameobject)
    {
        component.Button = gameobject.GetComponent<Button>();
        if (component.Button == null)
        {
            return false;
        }
        return true;
    }
}
