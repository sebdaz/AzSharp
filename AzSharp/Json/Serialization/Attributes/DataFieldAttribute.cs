using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class DataFieldAttribute : Attribute
{
    public readonly string Tag;
    public readonly bool Required;
    public readonly Type TypeSerializer;
    public DataFieldAttribute(string tag, Type? typeSerializer = null, bool required = false)
    {
        if (typeSerializer == null)
        {
            typeSerializer = typeof(ValueTypeSerializer);
        }
        Tag = tag;
        TypeSerializer = typeSerializer;
        Required = required;
    }
}
