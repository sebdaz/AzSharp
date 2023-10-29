using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCMeshFilterSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCMeshFilter, ComponentInit<UCMeshFilter>>(InitCallback);
        eventManager.SubscribeLocal<UCMeshFilter, ComponentDestroy<UCMeshFilter>>(DestroyCallback);
        eventManager.SubscribeLocal<UCMeshFilter, ComponentPostDeserialize<UCMeshFilter>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCMeshFilter> comp, ComponentPostDeserialize<UCMeshFilter> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.MeshFilter = transf.comp.GameObject.AddComponent<MeshFilter>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCMeshFilter> comp, ComponentInit<UCMeshFilter> args, uint entity_id)
    {
        if (comp.comp.MeshFilter != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.MeshFilter = gameobject.AddComponent<MeshFilter>();
    }

    private static void DestroyCallback(Component<UCMeshFilter> comp, ComponentDestroy<UCMeshFilter> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.MeshFilter);
        comp.comp.MeshFilter = null;
    }
}
