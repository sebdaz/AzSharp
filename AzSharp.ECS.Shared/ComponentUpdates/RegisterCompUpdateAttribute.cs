using System;

namespace AzSharp.ECS.Shared.ComponentUpdates;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterCompUpdateAttribute : Attribute
{
    public Type CompType { get; }
    public RegisterCompUpdateAttribute(Type compType)
    {
        CompType = compType;
    }
}
