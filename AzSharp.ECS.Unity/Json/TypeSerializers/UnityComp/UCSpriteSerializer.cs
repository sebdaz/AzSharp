using AzSharp.ECS.Unity.UnityComp;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using AzSharp.ECS.Unity.UnityComp;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCSpriteDataCache
{
    public string sprite;
    public UCSpriteDataCache(string sprite)
    {
        this.sprite = sprite;
    }
}

public sealed class UCSpriteSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCSprite cast = (UCSprite)obj;
        var dict = node.AsDict();

        string sprite_path = JsonSerializer.Deserialize<string, ValueTypeSerializer>(null, dict["Sprite"]);

        UCSpriteDataCache cache = new(sprite_path);
        cast.cache = cache;

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        UCSprite cast = (UCSprite)obj;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        dict.Add("Sprite", new JsonNode("Images/Circle.png"));
        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
