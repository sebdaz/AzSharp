using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, Inherited = false)]
public sealed class JsonSerializableAttribute : Attribute
{
    public readonly Type TypeSerializer;
    public JsonSerializableAttribute(Type typeSerializer)
    {
        TypeSerializer = typeSerializer;
    }
}
