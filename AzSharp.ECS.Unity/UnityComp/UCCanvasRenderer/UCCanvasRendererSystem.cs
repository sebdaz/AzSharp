using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCCanvasRendererSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCCanvasRenderer, ComponentInit<UCCanvasRenderer>>(InitCallback);
        eventManager.SubscribeLocal<UCCanvasRenderer, ComponentDestroy<UCCanvasRenderer>>(DestroyCallback);
        eventManager.SubscribeLocal<UCCanvasRenderer, ComponentPostDeserialize<UCCanvasRenderer>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCCanvasRenderer> comp, ComponentPostDeserialize<UCCanvasRenderer> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.CanvasRenderer = transf.comp.GameObject.AddComponent<CanvasRenderer>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCCanvasRenderer> comp, ComponentInit<UCCanvasRenderer> args, uint entity_id)
    {
        if (comp.comp.CanvasRenderer != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.CanvasRenderer = gameobject.AddComponent<CanvasRenderer>();
    }

    private static void DestroyCallback(Component<UCCanvasRenderer> comp, ComponentDestroy<UCCanvasRenderer> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.CanvasRenderer);
        comp.comp.CanvasRenderer = null;
    }
}
