using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCVerticalLayoutSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCVerticalLayout, ComponentInit<UCVerticalLayout>>(InitCallback);
        eventManager.SubscribeLocal<UCVerticalLayout, ComponentDestroy<UCVerticalLayout>>(DestroyCallback);
        eventManager.SubscribeLocal<UCVerticalLayout, ComponentPostDeserialize<UCVerticalLayout>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCVerticalLayout> comp, ComponentPostDeserialize<UCVerticalLayout> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.VerticalLayout = transf.comp.GameObject.AddComponent<VerticalLayoutGroup>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCVerticalLayout> comp, ComponentInit<UCVerticalLayout> args, uint entity_id)
    {
        if (comp.comp.VerticalLayout != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform>? transform = comp_manager.GetComponent<UCTransform>(entity_id);
        if (transform == null)
        {
            throw new ArgumentException("UCTransform not found on an entity");
        }
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.VerticalLayout = gameobject.AddComponent<VerticalLayoutGroup>();
    }

    private static void DestroyCallback(Component<UCVerticalLayout> comp, ComponentDestroy<UCVerticalLayout> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform>? transform = comp_manager.GetComponent<UCTransform>(entity_id);
        if (transform == null)
        {
            throw new ArgumentException("UCTransform not found on an entity");
        }
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.VerticalLayout);
        comp.comp.VerticalLayout = null;
    }
}
