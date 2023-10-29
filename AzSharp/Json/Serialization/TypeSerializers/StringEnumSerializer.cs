using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AzSharp.Json.Serialization.TypeSerializers;

public sealed class StringEnumSerializer<T> : ITypeSerializer
    where T : Enum
{
    private Dictionary<string, T> nameValueDict = new();
    private Dictionary<T, string> valueNameDict = new();
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        EnsureDictionary();
        string string_key = node.AsString();
        if (!nameValueDict.ContainsKey(string_key))
        {
            throw new ArgumentException("Tried to deserialize a string enum that is not right");
        }
        return nameValueDict[string_key];
    }

    public JsonNode Serialize(object obj, Type type)
    {
        EnsureDictionary();
        T enum_value = (T)obj;
        if (!valueNameDict.ContainsKey(enum_value))
        {
            throw new ArgumentException("Tried to serialize a string enum that is not right");
        }
        JsonNode node = new JsonNode(valueNameDict[enum_value]);
        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
    private void EnsureDictionary()
    {
        if (nameValueDict.Count != 0)
        {
            return;
        }
        Type enumType = typeof(T);

        var enumValues = enumType.GetEnumValues();

        string type_name = enumType.Name;
        foreach (T value in enumValues)
        {
            string string_name = $"{type_name}.{value}";
            nameValueDict[string_name] = value;
            valueNameDict[value] = string_name;
        }
    }
}
