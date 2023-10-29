using System;
using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.IoC;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCTextMeshProSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCTextMeshPro, ComponentInit<UCTextMeshPro>>(InitCallback);
        eventManager.SubscribeLocal<UCTextMeshPro, ComponentDestroy<UCTextMeshPro>>(DestroyCallback);
        eventManager.SubscribeLocal<UCTextMeshPro, ComponentPostDeserialize<UCTextMeshPro>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCTextMeshPro> comp, ComponentPostDeserialize<UCTextMeshPro> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.Text = transf.comp.GameObject.AddComponent<TextMeshPro>();
        TextMeshPro text = comp.comp.Text;
        UCTextMeshProDataCache? datacache = comp.comp.dataCache;
        if (datacache == null)
        {
            return;
        }
        text.text = datacache.text;
        text.fontSize = datacache.fontSize;
        text.horizontalAlignment = datacache.horizontalAlignment;
        text.verticalAlignment = datacache.verticalAlignment;
        text.fontStyle = datacache.fontStyles;

        comp.comp.dataCache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCTextMeshPro> comp, ComponentInit<UCTextMeshPro> args, uint entity_id)
    {
        if (comp.comp.Text != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.Text = gameobject.AddComponent<TextMeshPro>();
    }

    private static void DestroyCallback(Component<UCTextMeshPro> comp, ComponentDestroy<UCTextMeshPro> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Text);
        comp.comp.Text = null;
    }
}
