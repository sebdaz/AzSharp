using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities;

public enum EntityState : byte
{
    UNINITIALIZED,
    INITIALIZED,
    DESTROYED
}

public class Entity
{
    public const uint NULL_ENTITY = uint.MaxValue;

    private uint id = NULL_ENTITY;
    private EntityState state = EntityState.UNINITIALIZED;

    public uint ID { get { return id; } }
    public EntityState State
    {
        get { return state; }
        set { state = value; }
    }
    public Entity(uint id)
    {
        this.id = id;
    }
}
