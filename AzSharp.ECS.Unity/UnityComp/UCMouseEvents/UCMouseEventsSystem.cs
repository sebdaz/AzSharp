using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.Mono;
using AzSharp.IoC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCMouseEventsSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCMouseEvents, ComponentInit<UCMouseEvents>>(InitCallback);
        eventManager.SubscribeLocal<UCMouseEvents, ComponentDestroy<UCMouseEvents>>(DestroyCallback);
        eventManager.SubscribeLocal<UCMouseEvents, ComponentPostDeserialize<UCMouseEvents>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCMouseEvents> comp, ComponentPostDeserialize<UCMouseEvents> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.MouseEvents = transf.comp.GameObject.AddComponent<EntityMouseEvents>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCMouseEvents> comp, ComponentInit<UCMouseEvents> args, uint entity_id)
    {
        if (comp.comp.MouseEvents != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.MouseEvents = gameobject.AddComponent<EntityMouseEvents>();
    }

    private static void DestroyCallback(Component<UCMouseEvents> comp, ComponentDestroy<UCMouseEvents> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.MouseEvents);
        comp.comp.MouseEvents = null;
    }
}
