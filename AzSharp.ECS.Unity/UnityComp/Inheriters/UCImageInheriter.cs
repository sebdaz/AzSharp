using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCImageInheriter : UnityCompInheriter<UCImage>
{
    public override bool LoadFromSceneObject(UCImage component, GameObject gameobject)
    {
        component.image = gameobject.GetComponent<Image>();
        if (component.image == null)
        {
            return false;
        }
        return true;
    }
}
