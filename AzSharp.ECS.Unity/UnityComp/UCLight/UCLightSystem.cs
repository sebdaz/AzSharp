using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.IoC;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCLightSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCLight, ComponentInit<UCLight>>(InitCallback);
        eventManager.SubscribeLocal<UCLight, ComponentDestroy<UCLight>>(DestroyCallback);
        eventManager.SubscribeLocal<UCLight, ComponentPostDeserialize<UCLight>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCLight> comp, ComponentPostDeserialize<UCLight> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Light = transf.comp.GameObject.AddComponent<Light>();
        Light light = comp.comp.Light;
        UCLightDataCache? cache = comp.comp.dataCache;
        if (cache == null)
        {
            return;
        }
        light.type = cache.lightType;
        light.color = cache.color;
        light.cullingMask = cache.cullingMask;
        light.lightShadowCasterMode = cache.lightShadowCasterMode;
        light.intensity = cache.intensity;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCLight> comp, ComponentInit<UCLight> args, uint entity_id)
    {
        if (comp.comp.Light != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Light = gameobject.AddComponent<Light>();
    }

    private static void DestroyCallback(Component<UCLight> comp, ComponentDestroy<UCLight> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Light);
        comp.comp.Light = null;
    }
}
