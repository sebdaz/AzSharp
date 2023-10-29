using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCSliderInheriter : UnityCompInheriter<UCSlider>
{
    public override bool LoadFromSceneObject(UCSlider component, GameObject gameobject)
    {
        component.Slider = gameobject.GetComponent<Slider>();
        if (component.Slider == null)
        {
            return false;
        }
        return true;
    }
}
