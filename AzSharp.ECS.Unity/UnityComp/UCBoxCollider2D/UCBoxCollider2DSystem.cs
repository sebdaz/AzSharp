using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCBoxCollider2DSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCBoxCollider2D, ComponentInit<UCBoxCollider2D>>(InitCallback);
        eventManager.SubscribeLocal<UCBoxCollider2D, ComponentDestroy<UCBoxCollider2D>>(DestroyCallback);
        eventManager.SubscribeLocal<UCBoxCollider2D, ComponentPostDeserialize<UCBoxCollider2D>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCBoxCollider2D> comp, ComponentPostDeserialize<UCBoxCollider2D> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.BoxCollider = transf.comp.GameObject.AddComponent<BoxCollider2D>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCBoxCollider2D> comp, ComponentInit<UCBoxCollider2D> args, uint entity_id)
    {
        if (comp.comp.BoxCollider != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.BoxCollider = gameobject.AddComponent<BoxCollider2D>();
    }

    private static void DestroyCallback(Component<UCBoxCollider2D> comp, ComponentDestroy<UCBoxCollider2D> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.BoxCollider);
        comp.comp.BoxCollider = null;
    }
}
