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
public class UCImageSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCImage, ComponentInit<UCImage>>(InitCallback);
        eventManager.SubscribeLocal<UCImage, ComponentDestroy<UCImage>>(DestroyCallback);
        eventManager.SubscribeLocal<UCImage, ComponentPostDeserialize<UCImage>>(PostDeserializeCallback);
    }

    private void PostDeserializeCallback(Component<UCImage> comp, ComponentPostDeserialize<UCImage> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.image = transf.comp.GameObject.AddComponent<Image>();
        Image image = comp.comp.image;
        UCImageDataCache? cache = comp.comp.dataCache;
        if (cache == null)
        {
            return;
        }
        image.color = cache.color;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCImage> comp, ComponentInit<UCImage> args, uint entity_id)
    {
        if (comp.comp.image != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.image = gameobject.AddComponent<Image>();
    }

    private static void DestroyCallback(Component<UCImage> comp, ComponentDestroy<UCImage> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.image);
        comp.comp.image = null;
    }
}
