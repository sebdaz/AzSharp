using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.UnityComp.Inheriters;

public sealed class UCMeshFilterInheriter : UnityCompInheriter<UCMeshFilter>
{
    public override bool LoadFromSceneObject(UCMeshFilter component, GameObject gameobject)
    {
        component.MeshFilter = gameobject.GetComponent<MeshFilter>();
        if (component.MeshFilter == null)
        {
            return false;
        }
        return true;
    }
}
