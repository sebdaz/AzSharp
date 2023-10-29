using System;

namespace AzSharp.ECS.Shared.Components;

public sealed class ComponentPriority
{
    public Type componentType;
    public int priority;
    public ComponentPriority(Type componentType, int priority)
    {
        this.componentType = componentType;
        this.priority = priority;
    }
}
