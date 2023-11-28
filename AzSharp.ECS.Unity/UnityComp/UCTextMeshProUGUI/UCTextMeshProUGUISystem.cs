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
public class UCTextMeshProUGUISystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCTextMeshProUGUI, ComponentInit<UCTextMeshProUGUI>>(InitCallback);
        eventManager.SubscribeLocal<UCTextMeshProUGUI, ComponentDestroy<UCTextMeshProUGUI>>(DestroyCallback);
        eventManager.SubscribeLocal<UCTextMeshProUGUI, ComponentPostDeserialize<UCTextMeshProUGUI>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCTextMeshProUGUI> comp, ComponentPostDeserialize<UCTextMeshProUGUI> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.text = transf.comp.GameObject.AddComponent<TextMeshProUGUI>();
        TextMeshProUGUI text = comp.comp.Text;
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

    private static void InitCallback(Component<UCTextMeshProUGUI> comp, ComponentInit<UCTextMeshProUGUI> args, uint entity_id)
    {
        if (comp.comp.Text != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.text = gameobject.AddComponent<TextMeshProUGUI>();
    }

    private static void DestroyCallback(Component<UCTextMeshProUGUI> comp, ComponentDestroy<UCTextMeshProUGUI> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Text);
        comp.comp.text = null;
    }
}
