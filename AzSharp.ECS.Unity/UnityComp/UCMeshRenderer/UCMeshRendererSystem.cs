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
public class UCMeshRendererSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCMeshRenderer, ComponentInit<UCMeshRenderer>>(InitCallback);
        eventManager.SubscribeLocal<UCMeshRenderer, ComponentDestroy<UCMeshRenderer>>(DestroyCallback);
        eventManager.SubscribeLocal<UCMeshRenderer, ComponentPostDeserialize<UCMeshRenderer>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCMeshRenderer> comp, ComponentPostDeserialize<UCMeshRenderer> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.MeshRenderer = transf.comp.GameObject.AddComponent<MeshRenderer>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCMeshRenderer> comp, ComponentInit<UCMeshRenderer> args, uint entity_id)
    {
        if (comp.comp.MeshRenderer != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.MeshRenderer = gameobject.AddComponent<MeshRenderer>();
    }

    private static void DestroyCallback(Component<UCMeshRenderer> comp, ComponentDestroy<UCMeshRenderer> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.MeshRenderer);
        comp.comp.MeshRenderer = null;
    }
}
