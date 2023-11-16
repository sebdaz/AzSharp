using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using AzSharp.ECS.Shared.Events;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public sealed class ProtoDataEventRaiser<T> : IProtoDataApplier
    where T : IEntityPrototypeData
{
    public void RaiseApplyDataEvent(uint entity_id, IEntityPrototypeData data)
    {
        IoCManager.Resolve<IEventManager>().RaiseEvent(new ApplyPrototypeData<T>((T)data), entity_id);
    }
}
