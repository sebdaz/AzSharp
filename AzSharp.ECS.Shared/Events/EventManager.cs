using AzSharp.Info;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using AzSharp.ECS.Shared.Components;
using System.Text;
using AzSharp.ECS.Shared.Entities;

namespace AzSharp.ECS.Shared.Events;

public enum SubscriptionType : byte
{
    LOCAL,
    GLOBAL
}

public struct SubscriptionInstance
{
    public IEventSubscription sub;
    public SubscriptionType type;
    public SubscriptionInstance(IEventSubscription sub, SubscriptionType type)
    {
        this.sub = sub;
        this.type = type;
    }
}

public interface IEventSubscription
{
    public void GlobalCallback(IComponentManager manager, Event args, uint entity_id);
    public void LocalCallback(IComponentManager manager, Event args, uint entity_id);
    public int GetPriority();
}

public class EventSubscription<ComponentType, EventType> : IEventSubscription
        where EventType : Event
{
    public EventCallback<ComponentType, EventType> callback;
    public int priority = 0;
    public EventSubscription(EventCallback<ComponentType, EventType> callback, int priority)
    {
        this.callback = callback;
        this.priority = priority;
    }
    private void CallbackSubscriber(Component<ComponentType> comp, Event args, uint entity_id)
    {
        callback(comp, (EventType)args, entity_id);
    }
    public void GlobalCallback(IComponentManager manager, Event args, uint entity_id)
    {
        foreach (var iterated_component in manager.GetAllComponents<ComponentType>())
        {
            CallbackSubscriber(iterated_component, args, entity_id);
        }
    }
    public void LocalCallback(IComponentManager manager, Event args, uint entity_id)
    {
        Component<ComponentType>? comp = manager.GetComponent<ComponentType>(entity_id);
        if (comp == null)
        {
            throw new ArgumentException("Component is null during local callback");
        }
        CallbackSubscriber(comp, args, entity_id);
    }
    public int GetPriority()
    {
        return priority;
    }
}

public interface IComponentSublist
{
    public List<IEventSubscription> GetAllSubscribers();
}

public class ComponentSublist<EventType, ComponentType> : IComponentSublist
    where EventType : Event
{
    public List<EventSubscription<ComponentType, EventType>> list = new();
    public void AddSubscription(EventCallback<ComponentType, EventType> callback, int priority)
    {
        list.Add(new EventSubscription<ComponentType, EventType>(callback, priority));
    }

    public List<IEventSubscription> GetAllSubscribers()
    {
        List<IEventSubscription> sublist = new();
        foreach (var sub in list)
        {
            sublist.Add(sub);
        }
        return sublist;
    }
}

public interface IEventChannel { }

public class EventChannel<EventType> : IEventChannel
    where EventType : Event
{
    public Dictionary<Type, IComponentSublist> local_sublist = new();
    public Dictionary<Type, IComponentSublist> global_subs = new();

    public void AddGlobalSubscription<ComponentType>(EventCallback<ComponentType, EventType> callback, int priority)
    {
        Type comp_type = typeof(ComponentType);
        if (!global_subs.ContainsKey(comp_type))
        {
            global_subs[comp_type] = new ComponentSublist<EventType, ComponentType>();
        }
        ComponentSublist<EventType, ComponentType> sublist = (ComponentSublist<EventType, ComponentType>)global_subs[comp_type];
        sublist.AddSubscription(callback, priority);
    }

    public void AddLocalSubscription<ComponentType>(EventCallback<ComponentType, EventType> callback, int priority)
    {
        Type comp_type = typeof(ComponentType);
        if (!local_sublist.ContainsKey(comp_type))
        {
            local_sublist[comp_type] = new ComponentSublist<EventType, ComponentType>();
        }
        ComponentSublist<EventType, ComponentType> sublist = (ComponentSublist<EventType, ComponentType>)local_sublist[comp_type];
        sublist.AddSubscription(callback, priority);
    }
    public void RaiseEvent(EventType args, uint entity_id)
    {
        List<SubscriptionInstance> subscriptions = new();
        //global callbacks
        foreach (var pair in global_subs)
        {
            foreach (var sub in pair.Value.GetAllSubscribers())
            {
                subscriptions.Add(new SubscriptionInstance(sub, SubscriptionType.GLOBAL));
            }
        }
        //local callbacks
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        HashSet<Type>? ent_signature = comp_manager.GetEntitySignature(entity_id);
        if (ent_signature != null)
        {
            foreach (Type type in ent_signature)
            {
                if (!local_sublist.ContainsKey(type))
                {
                    continue;
                }
                IComponentSublist sublist = local_sublist[type];
                foreach (var sub in sublist.GetAllSubscribers())
                {
                    subscriptions.Add(new SubscriptionInstance(sub, SubscriptionType.LOCAL));
                }
            }
        }
        // Early return if nothing wants to listen to that event
        if (subscriptions.Count == 0)
        {
            return;
        }
        // Sort subscriptions by priority
        subscriptions = subscriptions.OrderBy(sub_instance => -sub_instance.sub.GetPriority()).ToList();
        // Call them by their local/global case
        foreach (SubscriptionInstance instance in subscriptions)
        {
            switch (instance.type)
            {
                case SubscriptionType.LOCAL:
                    {
                        instance.sub.LocalCallback(comp_manager, args, entity_id);
                        break;
                    }
                case SubscriptionType.GLOBAL:
                    {
                        instance.sub.GlobalCallback(comp_manager, args, entity_id);
                        break;
                    }
            }
        }
    }
}

public class EventManager : IEventManager
{
    private Dictionary<Type, IEventChannel> event_channels = new();
    private void EnsureChannelExistance<EventType>()
        where EventType : Event
    {
        Type event_type = typeof(EventType);
        if (event_channels.ContainsKey(event_type))
        {
            return;
        }
        event_channels[event_type] = new EventChannel<EventType>();
    }
    private EventChannel<EventType> GetEventChannel<EventType>()
        where EventType : Event
    {
        EnsureChannelExistance<EventType>();
        Type event_type = typeof(EventType);
        return (EventChannel<EventType>)event_channels[event_type];
    }
    public void RaiseEvent<EventType>(EventType args, uint entity_id)
        where EventType : Event
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        Entity ent = ent_manager.GetEntity(entity_id);
        if (ent == null)
        {
            InfoFunc.PrintInfo("Tried to raise an event on a non existing entity", InfoType.ERROR);
            return;
        }
        EventChannel<EventType> channel = GetEventChannel<EventType>();
        if (channel == null)
        {
            return;
        }
        channel.RaiseEvent(args, entity_id);
    }

    public void SubscribeGlobal<ComponentType, EventType>(EventCallback<ComponentType, EventType> callback, int priority = 0)
        where EventType : Event
    {
        EnsureChannelExistance<EventType>();
        EventChannel<EventType> channel = GetEventChannel<EventType>();
        channel.AddGlobalSubscription<ComponentType>(callback, priority);
    }

    public void SubscribeLocal<ComponentType, EventType>(EventCallback<ComponentType, EventType> callback, int priority = 0)
        where EventType : Event
    {
        EnsureChannelExistance<EventType>();
        EventChannel<EventType> channel = GetEventChannel<EventType>();
        channel.AddLocalSubscription<ComponentType>(callback, priority);
    }
}
