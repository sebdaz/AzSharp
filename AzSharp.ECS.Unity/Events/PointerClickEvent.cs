﻿using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerClickEvent : Event
{
    public PointerEventData eventData;
    public PointerClickEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

