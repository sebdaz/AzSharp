using System;

namespace AzSharp.ECS.Shared.ComponentUpdates;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterCompUpdateAttribute : Attribute
{
    public Type CompType { get; }
    public int Priority { get; }
    public RegisterCompUpdateAttribute(Type compType, int priority = 0)
    {
        CompType = compType;
        Priority = priority;
    }
}
