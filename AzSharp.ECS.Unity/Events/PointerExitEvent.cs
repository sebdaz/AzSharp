using AzSharp.ECS.Shared.Events;
using UnityEngine.EventSystems;

namespace AzSharp.ECS.Unity.Events;

public sealed class PointerExitEvent : Event
{
    public PointerEventData eventData;
    public PointerExitEvent(PointerEventData eventData)
    {
        this.eventData = eventData;
    }
}

