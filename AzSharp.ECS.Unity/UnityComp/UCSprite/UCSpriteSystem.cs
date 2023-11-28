using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.UnityComp;

[RegisterSystem]
public class UCSpriteSystem : ISystem
{
    public void Initialize()
    {
        IEventManager eventManager = IoCManager.Resolve<IEventManager>();
        eventManager.SubscribeLocal<UCSprite, ComponentInit<UCSprite>>(InitCallback);
        eventManager.SubscribeLocal<UCSprite, ComponentDestroy<UCSprite>>(DestroyCallback);
        eventManager.SubscribeLocal<UCSprite, ComponentPostDeserialize<UCSprite>>(PostDeserializeCallback);
    }

    private static void PostDeserializeCallback(Component<UCSprite> comp, ComponentPostDeserialize<UCSprite> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transf = comp_manager.AssumeGetComponent<UCTransform>(comp.entityID);
        comp.comp.sprite = transf.comp.GameObject.AddComponent<SpriteRenderer>();
        comp.comp.sprite.sprite = Resources.Load<Sprite>(comp.comp.cache.sprite);
        comp.comp.cache = null;
    }

    public void Shutdown()
    {
        return;
    }

    private static void InitCallback(Component<UCSprite> comp, ComponentInit<UCSprite> args, uint entity_id)
    {
        if (comp.comp.Sprite != null)
        {
            return;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject gameobject = transform.comp.GameObject;
        comp.comp.sprite = gameobject.AddComponent<SpriteRenderer>();
    }

    private static void DestroyCallback(Component<UCSprite> comp, ComponentDestroy<UCSprite> args, uint entity_id)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        Component<UCTransform> transform = comp_manager.AssumeGetComponent<UCTransform>(entity_id);
        GameObject game_object = transform.comp.GameObject;
        GameObject.Destroy(comp.comp.Sprite);
        comp.comp.sprite = null;
    }
}

