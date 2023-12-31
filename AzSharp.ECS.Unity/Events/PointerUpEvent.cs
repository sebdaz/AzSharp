﻿using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerUpEvent : Event
{
    public PointerEventData eventData;
    public PointerUpEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

