using AzSharp.ECS.Shared.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.ComponentUpdates;

public abstract class CompUpdateSystem<T> : ICompUpdateInterface
{
    public void Update(IComponent component, float delta_time)
    {
        UpdateComponent((Component<T>)component, delta_time);
    }
    public abstract void UpdateComponent(Component<T> component, float delta_time);
}
