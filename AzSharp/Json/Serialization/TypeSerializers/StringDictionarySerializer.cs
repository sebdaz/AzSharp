using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public class StringDictionarySerializer<TValue, TValueSerializer> : ITypeSerializer
    where TValueSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        Dictionary<string, TValue> dictionary = new();
        foreach (var pair in node.AsDict())
        {
            TValue? value = JsonSerializer.Deserialize<TValue, TValueSerializer>(null, pair.Value, version);
#pragma warning disable CS8604 // Possible null reference argument.
            dictionary.Add(pair.Key, value);
#pragma warning restore CS8604 // Possible null reference argument.
        }
        return dictionary;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        Dictionary<string, TValue> cast = (Dictionary<string, TValue>)obj;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var node_dict = node.AsDict();
        foreach (var pair in cast)
        {
            JsonNode value_node = JsonSerializer.Serialize<TValue, TValueSerializer>(pair.Value);
            node_dict[pair.Key] = value_node;
        }
        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
