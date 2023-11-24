using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using AzSharp.ECS.Shared.Systems;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public sealed class UCRectMask2DSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCRectMask2D, ComponentInit<UCRectMask2D>>(InitCallback);
        eventManager.SubscribeLocal<UCRectMask2D, ComponentDestroy<UCRectMask2D>>(DestroyCallback);
        eventManager.SubscribeLocal<UCRectMask2D, ComponentPostDeserialize<UCRectMask2D>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCRectMask2D> comp, ComponentPostDeserialize<UCRectMask2D> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.rectMask2D = transf.comp.GameObject.AddComponent<RectMask2D>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCRectMask2D> comp, ComponentInit<UCRectMask2D> args, uint entity_id)
    {
        if (comp.comp.rectMask2D != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.rectMask2D = gameobject.AddComponent<RectMask2D>();
    }

    private static void DestroyCallback(Component<UCRectMask2D> comp, ComponentDestroy<UCRectMask2D> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.rectMask2D);
        comp.comp.rectMask2D = null;
    }
}
