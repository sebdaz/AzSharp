using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.IoC;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCCameraSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCCamera, ComponentInit<UCCamera>>(InitCallback);
        eventManager.SubscribeLocal<UCCamera, ComponentDestroy<UCCamera>>(DestroyCallback);
        eventManager.SubscribeLocal<UCCamera, ComponentPostDeserialize<UCCamera>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCCamera> comp, ComponentPostDeserialize<UCCamera> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.camera = transf.comp.GameObject.AddComponent<Camera>();
        Camera camera = comp.comp.camera;
        UCCameraDataCache? cache = comp.comp.dataCache;
        if (cache == null)
        {
            return;
        }
        camera.clearFlags = cache.clearFlags;
        camera.backgroundColor = cache.backgroundColor;
        camera.cullingMask = cache.cullingMask;
        camera.projectionMatrix = cache.projectionMatrix;
        camera.fieldOfView = cache.fieldOfView;
        camera.nearClipPlane = cache.nearClipPlane;
        camera.farClipPlane = cache.farClipPlane;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCCamera> comp, ComponentInit<UCCamera> args, uint entity_id)
    {
        if (comp.comp.camera != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.camera = gameobject.AddComponent<Camera>();
    }

    private static void DestroyCallback(Component<UCCamera> comp, ComponentDestroy<UCCamera> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.camera);
        comp.comp.camera = null;
    }
}
