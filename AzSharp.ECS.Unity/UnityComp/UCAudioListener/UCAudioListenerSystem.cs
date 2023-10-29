using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCAudioListenerSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCAudioListener, ComponentInit<UCAudioListener>>(InitCallback);
        eventManager.SubscribeLocal<UCAudioListener, ComponentDestroy<UCAudioListener>>(DestroyCallback);
        eventManager.SubscribeLocal<UCAudioListener, ComponentPostDeserialize<UCAudioListener>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCAudioListener> comp, ComponentPostDeserialize<UCAudioListener> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.AudioListener = transf.comp.GameObject.AddComponent<AudioListener>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCAudioListener> comp, ComponentInit<UCAudioListener> args, uint entity_id)
    {
        if (comp.comp.AudioListener != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.AudioListener = gameobject.AddComponent<AudioListener>();
    }

    private static void DestroyCallback(Component<UCAudioListener> comp, ComponentDestroy<UCAudioListener> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.AudioListener);
        comp.comp.AudioListener = null;
    }
}
