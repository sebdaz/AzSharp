using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public interface IProtoDataApplier
{
    void RaiseApplyDataEvent(uint entity_id, IEntityPrototypeData data);
}

public abstract class ProtoDataApplier<T> : IProtoDataApplier
{
    public void RaiseApplyDataEvent(uint entity_id, IEntityPrototypeData data)
    {
        ApplyData(entity_id, (T)data);
    }
    public abstract void ApplyData(uint entity_id, T data);
}