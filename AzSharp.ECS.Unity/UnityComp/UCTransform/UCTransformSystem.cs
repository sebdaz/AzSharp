using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.ECS.Unity.GameObjectManager;
using AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;
using AzSharp.IoC;
using System;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCTransformSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCTransform, ComponentDestroy<UCTransform>>(DestroyCallback);
        eventManager.SubscribeLocal<UCTransform, ComponentAttach<UCTransform>>(AttachCallback);
        eventManager.SubscribeLocal<UCTransform, ComponentPostDeserialize<UCTransform>>(PostDeserializeCallback);
        eventManager.SubscribeLocal<UCTransform, ComponentLatePostDeserialize<UCTransform>>(LatePostDeserializeCallback);
    }

    private static void LatePostDeserializeCallback(Component<UCTransform> comp, ComponentLatePostDeserialize<UCTransform> args, uint entity_id)
    {
        if (comp.comp.dataCache == null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        foreach (uint children in comp.comp.dataCache.childrenEntities)
        {
            Component<UCTransform> child_transf = comp_manager.AssumeGetComponent<UCTransform>(children);
            child_transf.comp.SetParent(comp.entityID, false);
        }
        comp.comp.dataCache = null;
    }

    private static void PostDeserializeCallback(Component<UCTransform> comp, ComponentPostDeserialize<UCTransform> args, uint entity_id)
    {
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        comp.comp.gameObject = go_manager.CreateGameObject(comp.entityID);
        UCTransformDataCache? datacache = comp.comp.dataCache;
        if (datacache == null)
        {
            return;
        }
        GameObject gameobject = comp.comp.gameObject;
        Transform transform = gameobject.transform;

        gameobject.name = datacache.name;
        gameobject.SetActive(datacache.active);

        transform.position = datacache.position;
        transform.rotation = datacache.rotation;
        transform.localScale = datacache.localScale;

        UCTransformRectDataCache? rectdata = datacache.rectData;
        if (rectdata != null)
        {
            RectTransform rect = gameobject.AddComponent<RectTransform>();
            rect.pivot = rectdata.pivot;
            rect.anchorMax = rectdata.anchorMax;
            rect.anchorMin = rectdata.anchorMin;
            rect.sizeDelta = rectdata.sizeDelta;
            rect.anchoredPosition3D = rectdata.anchoredPosition3D;
        }
    }

    public void Shutdown()
    {
        return;
    }

    private static void AttachCallback(Component<UCTransform> comp, ComponentAttach<UCTransform> args, uint entity_id)
    {
        comp.comp.AssertSignGameObject(comp.entityID);
    }

    private static void DestroyCallback(Component<UCTransform> comp, ComponentDestroy<UCTransform> args, uint entity_id)
    {
        comp.comp.DestroyAllChildren();
        comp.comp.DestroyGameObject();
    }

}
