using System;
using System.Collections.Generic;
using System.Text;
using AzSharp.ECS.Shared.Events;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public sealed class ApplyPrototypeData<T> : Event
    where T : IEntityPrototypeData
{
    public T Data;
    public ApplyPrototypeData(T data)
    {
        Data = data;
    }
}
