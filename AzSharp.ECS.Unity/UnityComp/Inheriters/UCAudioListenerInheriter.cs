using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCAudioListenerInheriter : UnityCompInheriter<UCAudioListener>
{
    public override bool LoadFromSceneObject(UCAudioListener component, GameObject gameobject)
    {
        component.AudioListener = gameobject.GetComponent<AudioListener>();
        if (component.AudioListener == null)
        {
            return false;
        }
        return true;
    }
}
