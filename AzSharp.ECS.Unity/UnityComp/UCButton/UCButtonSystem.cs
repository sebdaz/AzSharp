using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCButtonSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCButton, ComponentInit<UCButton>>(InitCallback);
        eventManager.SubscribeLocal<UCButton, ComponentDestroy<UCButton>>(DestroyCallback);
        eventManager.SubscribeLocal<UCButton, ComponentPostDeserialize<UCButton>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCButton> comp, ComponentPostDeserialize<UCButton> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Button = transf.comp.GameObject.AddComponent<Button>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCButton> comp, ComponentInit<UCButton> args, uint entity_id)
    {
        if (comp.comp.Button != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Button = gameobject.AddComponent<Button>();
    }

    private static void DestroyCallback(Component<UCButton> comp, ComponentDestroy<UCButton> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Button);
        comp.comp.Button = null;
    }
}
