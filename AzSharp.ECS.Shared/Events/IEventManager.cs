using System;
using System.Collections.Generic;
using System.Text;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Shared.Events;

public abstract class Event {};

public sealed class ComponentAttach<T> : Event { }
public sealed class ComponentDetach<T> : Event { }
public sealed class ComponentInit<T> : Event { }
public sealed class ComponentDestroy<T> : Event { }
public sealed class ComponentPostDeserialize<T> : Event { }
public sealed class ComponentLatePostDeserialize<T> : Event { }

public delegate void EventCallback<ComponentType, EventType>(Component<ComponentType> comp, EventType args, uint entity_id)
    where EventType : Event;

public interface IEventManager
{
    public void RaiseEvent<EventType>(EventType args, uint entity_id)
        where EventType : Event;
    public void SubscribeLocal<ComponentType, EventType>(EventCallback<ComponentType, EventType> callback, int priority = 0)
        where EventType : Event;
    public void SubscribeGlobal<ComponentType, EventType>(EventCallback<ComponentType, EventType> callback, int priority = 0)
        where EventType : Event;
}
