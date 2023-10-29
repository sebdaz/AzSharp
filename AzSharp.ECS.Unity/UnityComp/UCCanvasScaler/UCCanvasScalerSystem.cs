using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.IoC;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCCanvasScalerSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCCanvasScaler, ComponentInit<UCCanvasScaler>>(InitCallback);
        eventManager.SubscribeLocal<UCCanvasScaler, ComponentDestroy<UCCanvasScaler>>(DestroyCallback);
        eventManager.SubscribeLocal<UCCanvasScaler, ComponentPostDeserialize<UCCanvasScaler>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCCanvasScaler> comp, ComponentPostDeserialize<UCCanvasScaler> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.CanvasScaler = transf.comp.GameObject.AddComponent<CanvasScaler>();
        CanvasScaler scaler = comp.comp.CanvasScaler;
        UCCanvasScalerDataCache? cache = comp.comp.dataCache;
        if (cache == null)
        {
            return;
        }
        scaler.uiScaleMode = cache.scaleMode;
        scaler.referenceResolution = cache.referenceResolution;
        scaler.screenMatchMode = cache.screenMatchMode;
        scaler.matchWidthOrHeight = cache.matchWidthOrHeight;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCCanvasScaler> comp, ComponentInit<UCCanvasScaler> args, uint entity_id)
    {
        if (comp.comp.CanvasScaler != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.CanvasScaler = gameobject.AddComponent<CanvasScaler>();
    }

    private static void DestroyCallback(Component<UCCanvasScaler> comp, ComponentDestroy<UCCanvasScaler> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.CanvasScaler);
        comp.comp.CanvasScaler = null;
    }
}
