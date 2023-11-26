using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerScrollEvent : Event
{
    public PointerEventData eventData;
    public PointerScrollEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

