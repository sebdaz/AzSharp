namespace AzSharp.ECS.Shared.Components;

public interface IComponentEventRaiser
{
    void RaiseAttachEvent(uint entity_id);
    void RaiseDetachEvent(uint entity_id);
    void RaiseInitEvent(uint entity_id);
    void RaiseDestroyEvent(uint entity_id);
    void RaisePostDeserializeEvent(uint entity_id);
    void RaiseLatePostDeserializeEvent(uint entity_id);
}
