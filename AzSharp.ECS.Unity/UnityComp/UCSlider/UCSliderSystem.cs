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
public class UCSliderSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCSlider, ComponentInit<UCSlider>>(InitCallback);
        eventManager.SubscribeLocal<UCSlider, ComponentDestroy<UCSlider>>(DestroyCallback);
        eventManager.SubscribeLocal<UCSlider, ComponentPostDeserialize<UCSlider>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCSlider> comp, ComponentPostDeserialize<UCSlider> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Slider = transf.comp.GameObject.AddComponent<Slider>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCSlider> comp, ComponentInit<UCSlider> args, uint entity_id)
    {
        if (comp.comp.Slider != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Slider = gameobject.AddComponent<Slider>();
    }

    private static void DestroyCallback(Component<UCSlider> comp, ComponentDestroy<UCSlider> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Slider);
        comp.comp.Slider = null;
    }
}
