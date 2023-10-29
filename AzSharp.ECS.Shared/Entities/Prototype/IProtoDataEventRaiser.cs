using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public interface IProtoDataEventRaiser
{
    void RaiseApplyDataEvent(uint entity_id, IEntityPrototypeData data);
}

