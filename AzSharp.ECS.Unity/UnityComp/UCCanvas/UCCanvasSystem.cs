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
public class UCCanvasSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCCanvas, ComponentInit<UCCanvas>>(InitCallback);
        eventManager.SubscribeLocal<UCCanvas, ComponentDestroy<UCCanvas>>(DestroyCallback);
        eventManager.SubscribeLocal<UCCanvas, ComponentPostDeserialize<UCCanvas>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCCanvas> comp, ComponentPostDeserialize<UCCanvas> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Canvas = transf.comp.GameObject.AddComponent<Canvas>();
        Canvas canvas = comp.comp.Canvas;
        UCCanvasDataCache? cache = comp.comp.dataCache;
        if (cache == null)
        {
            return;
        }
        canvas.renderMode = cache.renderMode;
        canvas.pixelPerfect = cache.pixelPerfect;
        canvas.sortingOrder = cache.sortingOrder;
        canvas.targetDisplay = cache.targetDisplay;
        canvas.additionalShaderChannels = cache.additionalCanvasShaderChannels;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCCanvas> comp, ComponentInit<UCCanvas> args, uint entity_id)
    {
        if (comp.comp.Canvas != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Canvas = gameobject.AddComponent<Canvas>();
    }

    private static void DestroyCallback(Component<UCCanvas> comp, ComponentDestroy<UCCanvas> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Canvas);
        comp.comp.Canvas = null;
    }
}
