using System;

namespace AzSharp.Reflection;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
public sealed class ReflectAttribute : Attribute
{
    public bool Discoverable { get; }
    public ReflectAttribute(bool discoverable)
    {
        Discoverable = discoverable;
    }
}
