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
public class UCGraphicRaycasterSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCGraphicRaycaster, ComponentInit<UCGraphicRaycaster>>(InitCallback);
        eventManager.SubscribeLocal<UCGraphicRaycaster, ComponentDestroy<UCGraphicRaycaster>>(DestroyCallback);
        eventManager.SubscribeLocal<UCGraphicRaycaster, ComponentPostDeserialize<UCGraphicRaycaster>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCGraphicRaycaster> comp, ComponentPostDeserialize<UCGraphicRaycaster> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.GraphicRaycaster = transf.comp.GameObject.AddComponent<GraphicRaycaster>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCGraphicRaycaster> comp, ComponentInit<UCGraphicRaycaster> args, uint entity_id)
    {
        if (comp.comp.GraphicRaycaster != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.GraphicRaycaster = gameobject.AddComponent<GraphicRaycaster>();
    }

    private static void DestroyCallback(Component<UCGraphicRaycaster> comp, ComponentDestroy<UCGraphicRaycaster> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.GraphicRaycaster);
        comp.comp.GraphicRaycaster = null;
    }
}
