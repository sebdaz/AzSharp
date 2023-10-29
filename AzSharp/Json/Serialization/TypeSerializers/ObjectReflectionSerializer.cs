using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public class ObjectReflectionSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        if (obj == null)
        {
            obj = Activator.CreateInstance(type);
        }
        FieldInfo[] fieldInfo = type.GetFields();
        PropertyInfo[] propertyInfo = type.GetProperties();
        var dict = node.AsDict();
        if (dict == null)
        {
            return obj;
        }
        foreach (FieldInfo field in fieldInfo)
        {
            DataFieldAttribute attribute = field.GetCustomAttribute<DataFieldAttribute>();
            if (attribute == null)
            {
                continue;
            }
            if (!dict.ContainsKey(attribute.Tag))
            {
                if (attribute.Required)
                {
                    throw new ArgumentException($"Didn't find required data for serialization for object of type {type}");
                }
                continue;
            }
            object? value = JsonSerializer.Deserialize(field.GetValue(obj), dict[attribute.Tag], field.FieldType, attribute.TypeSerializer, version);
            field.SetValue(obj, value);
        }
        foreach (PropertyInfo property in propertyInfo)
        {
            DataFieldAttribute attribute = property.GetCustomAttribute<DataFieldAttribute>();
            if (attribute == null)
            {
                continue;
            }
            if (!dict.ContainsKey(attribute.Tag))
            {
                if (attribute.Required)
                {
                    throw new ArgumentException($"Didn't find required data for serialization for object of type {type}");
                }
                continue;
            }
            object? value = JsonSerializer.Deserialize(property.GetValue(obj), dict[attribute.Tag], property.PropertyType, attribute.TypeSerializer, version);
            property.SetValue(obj, value);
        }
        return obj;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        FieldInfo[] fieldInfo = type.GetFields();
        PropertyInfo[] propertyInfo = type.GetProperties();
        foreach (FieldInfo field in fieldInfo)
        {
            SerializeFieldOrPropertyMember(node, obj, field.GetValue(obj), field, field.FieldType);
        }
        foreach (PropertyInfo property in propertyInfo)
        {
            SerializeFieldOrPropertyMember(node, obj, property.GetValue(obj), property, property.PropertyType);
        }
        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
    private void SerializeFieldOrPropertyMember(JsonNode node, object obj, object value, MemberInfo member, Type member_type)
    {
        DataFieldAttribute attribute = member.GetCustomAttribute<DataFieldAttribute>();
        if (attribute == null)
        {
            return;
        }
        node.AsDict()[attribute.Tag] = JsonSerializer.Serialize(value, member_type, attribute.TypeSerializer);
    }
}
