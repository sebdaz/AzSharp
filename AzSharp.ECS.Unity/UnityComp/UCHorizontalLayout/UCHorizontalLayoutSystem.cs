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
public class UCHorizontalLayoutSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCHorizontalLayout, ComponentInit<UCHorizontalLayout>>(InitCallback);
        eventManager.SubscribeLocal<UCHorizontalLayout, ComponentDestroy<UCHorizontalLayout>>(DestroyCallback);
        eventManager.SubscribeLocal<UCHorizontalLayout, ComponentPostDeserialize<UCHorizontalLayout>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCHorizontalLayout> comp, ComponentPostDeserialize<UCHorizontalLayout> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.HorizontalLayout = transf.comp.GameObject.AddComponent<HorizontalLayoutGroup>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCHorizontalLayout> comp, ComponentInit<UCHorizontalLayout> args, uint entity_id)
    {
        if (comp.comp.HorizontalLayout != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.HorizontalLayout = gameobject.AddComponent<HorizontalLayoutGroup>();
    }

    private static void DestroyCallback(Component<UCHorizontalLayout> comp, ComponentDestroy<UCHorizontalLayout> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.HorizontalLayout);
        comp.comp.HorizontalLayout = null;
    }
}
