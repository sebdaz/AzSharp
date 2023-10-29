using AzSharp.ECS.Shared.Entities;

namespace AzSharp.ECS.Shared.Components;

public enum ComponentState : byte
{
    UNATTACHED,
    ATTACHED,
    INITIALIZED,
    DESTROYED
}


public static class ComponentConst
{
    public const uint NULL_COMP = uint.MaxValue;
}


public sealed class Component<T> : IComponent
{
    public uint compID = ComponentConst.NULL_COMP;
    public uint entityID = Entity.NULL_ENTITY;
    public ComponentState state = ComponentState.UNATTACHED;
    public T comp;
    public Component(uint compID, uint entityID, T comp)
    {
        this.compID = compID;
        this.entityID = entityID;
        this.comp = comp;
    }

    public uint ID()
    {
        return compID;
    }

    public uint EntityID()
    {
        return entityID;
    }

    public ComponentState State()
    {
        return state;
    }

    public object GetComponent()
    {
#pragma warning disable CS8603 // Possible null reference return.
        return comp;
#pragma warning restore CS8603 // Possible null reference return.
    }

    public void SetID(uint id)
    {
        compID = id;
    }

    public void SetEntityID(uint ent_id)
    {
        entityID = ent_id;
    }

    public void SetComponentState(ComponentState state)
    {
        this.state = state;
    }

    public void SetComponent(object component)
    {
        comp = (T)component;
    }
}
