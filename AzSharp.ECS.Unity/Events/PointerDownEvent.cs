using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerDownEvent : Event
{
    PointerEventData eventData;
    public PointerDownEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

