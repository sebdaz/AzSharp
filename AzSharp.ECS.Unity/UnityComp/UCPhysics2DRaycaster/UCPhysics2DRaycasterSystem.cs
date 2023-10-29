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
public class UCPhysics2DRaycasterSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCPhysics2DRaycaster, ComponentInit<UCPhysics2DRaycaster>>(InitCallback);
        eventManager.SubscribeLocal<UCPhysics2DRaycaster, ComponentDestroy<UCPhysics2DRaycaster>>(DestroyCallback);
        eventManager.SubscribeLocal<UCPhysics2DRaycaster, ComponentPostDeserialize<UCPhysics2DRaycaster>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCPhysics2DRaycaster> comp, ComponentPostDeserialize<UCPhysics2DRaycaster> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Physics2DRaycaster = transf.comp.GameObject.AddComponent<Physics2DRaycaster>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCPhysics2DRaycaster> comp, ComponentInit<UCPhysics2DRaycaster> args, uint entity_id)
    {
        if (comp.comp.Physics2DRaycaster != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Physics2DRaycaster = gameobject.AddComponent<Physics2DRaycaster>();
    }

    private static void DestroyCallback(Component<UCPhysics2DRaycaster> comp, ComponentDestroy<UCPhysics2DRaycaster> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Physics2DRaycaster);
        comp.comp.Physics2DRaycaster = null;
    }
}
