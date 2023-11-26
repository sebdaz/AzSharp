using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerEnterEvent : Event
{
    public PointerEventData eventData;
    public PointerEnterEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

