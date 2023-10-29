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
public class UCTMPInputFieldSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCTMPInputField, ComponentInit<UCTMPInputField>>(InitCallback);
        eventManager.SubscribeLocal<UCTMPInputField, ComponentDestroy<UCTMPInputField>>(DestroyCallback);
        eventManager.SubscribeLocal<UCTMPInputField, ComponentPostDeserialize<UCTMPInputField>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCTMPInputField> comp, ComponentPostDeserialize<UCTMPInputField> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.InputField = transf.comp.GameObject.AddComponent<TMP_InputField>();
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCTMPInputField> comp, ComponentInit<UCTMPInputField> args, uint entity_id)
    {
        if (comp.comp.InputField != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.InputField = gameobject.AddComponent<TMP_InputField>();
    }

    private static void DestroyCallback(Component<UCTMPInputField> comp, ComponentDestroy<UCTMPInputField> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.InputField);
        comp.comp.InputField = null;
    }
}
