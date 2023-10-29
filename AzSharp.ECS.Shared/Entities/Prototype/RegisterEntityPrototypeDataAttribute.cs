using System;

namespace AzSharp.ECS.Shared.Entities.Prototype;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterEntityPrototypeDataAttribute : Attribute
{
    public Type EventRaiserType;
    public RegisterEntityPrototypeDataAttribute(Type event_raiser_type)
    {
        EventRaiserType = event_raiser_type;
    }
}
