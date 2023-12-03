using AzSharp.ECS.Shared.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.ComponentUpdates;

public interface ICompUpdateManager
{
    public void RegisterCompUpdate<CompUpdate, ComponentType>(int priority)
        where CompUpdate : ICompUpdateInterface, new();
    public void RegisterCompUpdate(Type comp_update_type, Type comp_type, int priority);
    public void Update(float delta_time);
    public void RegisterFromAttributes();
}
