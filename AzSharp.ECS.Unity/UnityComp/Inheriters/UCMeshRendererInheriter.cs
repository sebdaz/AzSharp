using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCMeshRendererInheriter : UnityCompInheriter<UCMeshRenderer>
{
    public override bool LoadFromSceneObject(UCMeshRenderer component, GameObject gameobject)
    {
        component.MeshRenderer = gameobject.GetComponent<MeshRenderer>();
        if (component.MeshRenderer == null)
        {
            return false;
        }
        return true;
    }
}
