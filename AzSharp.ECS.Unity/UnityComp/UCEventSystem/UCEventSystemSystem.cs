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
public class UCEventSystemSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCEventSystem, ComponentInit<UCEventSystem>>(InitCallback);
        eventManager.SubscribeLocal<UCEventSystem, ComponentDestroy<UCEventSystem>>(DestroyCallback);
        eventManager.SubscribeLocal<UCEventSystem, ComponentPostDeserialize<UCEventSystem>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCEventSystem> comp, ComponentPostDeserialize<UCEventSystem> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.EventSystem = transf.comp.GameObject.AddComponent<EventSystem>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCEventSystem> comp, ComponentInit<UCEventSystem> args, uint entity_id)
    {
        if (comp.comp.EventSystem != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.EventSystem = gameobject.AddComponent<EventSystem>();
    }

    private static void DestroyCallback(Component<UCEventSystem> comp, ComponentDestroy<UCEventSystem> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.EventSystem);
        comp.comp.EventSystem = null;
    }
}
