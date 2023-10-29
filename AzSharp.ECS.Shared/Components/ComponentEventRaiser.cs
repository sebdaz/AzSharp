using AzSharp.IoC;
using AzSharp.ECS.Shared.Events;

namespace AzSharp.ECS.Shared.Components;

public class ComponentEventRaiser<T> : IComponentEventRaiser
{
    private ComponentAttach<T> attach_event = new();
    private ComponentDetach<T> detach_event = new();
    private ComponentInit<T> init_event = new();
    private ComponentDestroy<T> destroy_event = new();
    private ComponentPostDeserialize<T> post_deserialize_event = new();
    private ComponentLatePostDeserialize<T> late_post_deserialize_event = new();
    public void RaiseAttachEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(attach_event, entity_id);
    }

    public void RaiseDestroyEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(destroy_event, entity_id);
    }

    public void RaiseDetachEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(detach_event, entity_id);
    }

    public void RaiseInitEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(init_event, entity_id);
    }

    public void RaiseLatePostDeserializeEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(late_post_deserialize_event, entity_id);
    }

    public void RaisePostDeserializeEvent(uint entity_id)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(post_deserialize_event, entity_id);
    }
}