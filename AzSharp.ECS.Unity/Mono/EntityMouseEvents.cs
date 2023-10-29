using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Unity.GameObjectManager;
using AzSharp.IoC;
using AzSharp.ECS.Unity.Events;

namespace AzSharp.ECS.Unity.Mono;

public class EntityMouseEvents : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IScrollHandler
{
    public bool Draggable = false;
    public bool DraggableOnto = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerClickEvent(eventData));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerDownEvent(eventData));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerEnterEvent(eventData));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerExitEvent(eventData));
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerUpEvent(eventData));
    }

    public void OnScroll(PointerEventData eventData)
    {
        EmittMouseEvent(new PointerScrollEvent(eventData));
    }
    public void EmittMouseEvent<T>(T args)
        where T : Event
    {
        IGameObjectManager go_manager = IoCManager.Resolve<IGameObjectManager>();
        IEventManager event_manager = IoCManager.Resolve<IEventManager>();
        event_manager.RaiseEvent(args, go_manager.GetEntityID(gameObject));
    }
}
