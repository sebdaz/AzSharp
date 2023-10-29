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
public class UCStandaloneInputModuleSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCStandaloneInputModule, ComponentInit<UCStandaloneInputModule>>(InitCallback);
        eventManager.SubscribeLocal<UCStandaloneInputModule, ComponentDestroy<UCStandaloneInputModule>>(DestroyCallback);
        eventManager.SubscribeLocal<UCStandaloneInputModule, ComponentPostDeserialize<UCStandaloneInputModule>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCStandaloneInputModule> comp, ComponentPostDeserialize<UCStandaloneInputModule> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.InputModule = transf.comp.GameObject.AddComponent<StandaloneInputModule>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCStandaloneInputModule> comp, ComponentInit<UCStandaloneInputModule> args, uint entity_id)
    {
        if (comp.comp.InputModule != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.InputModule = gameobject.AddComponent<StandaloneInputModule>();
    }

    private static void DestroyCallback(Component<UCStandaloneInputModule> comp, ComponentDestroy<UCStandaloneInputModule> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.InputModule);
        comp.comp.InputModule = null;
    }
}
