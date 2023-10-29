using System;

namespace AzSharp.ECS.Shared.Components;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterComponentAttribute : Attribute
{
    public Type ContainerType { get; }
    public Type EventRaiserType { get; }
    public int InitPriority { get; }
    public RegisterComponentAttribute(Type container_type, Type event_raiser_type, int init_priority = 0)
    {
        ContainerType = container_type;
        EventRaiserType = event_raiser_type;
        InitPriority = init_priority;
    }
}
