using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;

namespace AzSharp.Json.Serialization;

public static class JsonSerializer
{
    private static Dictionary<Type, ITypeSerializer> typeDictionary = new();
    public  static ITypeSerializer GetSerializer(Type serializer_type)
    {
        if (!typeDictionary.ContainsKey(serializer_type))
        {
            typeDictionary[serializer_type] = (ITypeSerializer)Activator.CreateInstance(serializer_type);
        }
        return typeDictionary[serializer_type];
    }
    public static T GetSerializer<T>()
        where T : ITypeSerializer
    {
        return (T)GetSerializer(typeof(T));
    }
    public static JsonNode Serialize<TObject, TSerializer>(TObject obj)
        where TSerializer : ITypeSerializer
    {
#pragma warning disable CS8604 // Possible null reference argument.
        return Serialize(obj, typeof(TObject), typeof(TSerializer));
#pragma warning restore CS8604 // Possible null reference argument.
    }
    public static JsonNode Serialize(object obj, Type object_type, Type serializer_type)
    {
        if (obj == null)
        {
            throw new ArgumentException($"Obj is null when trying to serialize {object_type} with {serializer_type} serializer");
        }
        ITypeSerializer serializer = GetSerializer(serializer_type);
        return serializer.Serialize(obj, object_type);
    }
    public static TObject? Deserialize<TObject, TSerializer>(object? obj, JsonNode node, int version = 0)
        where TSerializer : ITypeSerializer
    {
        return (TObject?)Deserialize(obj, node, typeof(TObject), typeof(TSerializer), version);
    }
    public static object? Deserialize(object? obj, JsonNode node, Type object_type, Type serializer_type, int version = 0)
    {
        if (node == null)
        {
            throw new ArgumentException($"Json Node is null when trying to deserialize {object_type} with {serializer_type} serializer");
        }
        ITypeSerializer serializer = GetSerializer(serializer_type);
        serializer.VersionDataTreatment(obj, node, object_type, version);
        return serializer.Deserialize(node, obj, object_type, version);
    }
    public static void AssertObject(object? obj)
    {
        if (obj == null)
        {
            throw new ArgumentException("Obj is during serialization null");
        }
    }
}
