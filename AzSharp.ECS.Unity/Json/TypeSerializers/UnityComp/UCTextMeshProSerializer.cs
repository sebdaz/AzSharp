using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.IoC;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCTextMeshProDataCache
{
    public string text;
    public float fontSize;
    public VerticalAlignmentOptions verticalAlignment;
    public HorizontalAlignmentOptions horizontalAlignment;
    public FontStyles fontStyles;
    public UCTextMeshProDataCache(string text, float fontSize, VerticalAlignmentOptions verticalAlignment, HorizontalAlignmentOptions horizontalAlignment, FontStyles fontStyles)
    {
        this.text = text;
        this.fontSize = fontSize;
        this.verticalAlignment = verticalAlignment;
        this.horizontalAlignment = horizontalAlignment;
        this.fontStyles = fontStyles;
    }
}

public sealed class UCTextMeshProSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCTextMeshPro cast = (UCTextMeshPro)obj;
        var dict = node.AsDict();

        string? text = JsonSerializer.Deserialize<string, ValueTypeSerializer>(null, dict["Text"]);
        if (text == null)
        {
            throw new ArgumentException("Text is null during TMP deserialization");
        }
        float fontSize = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["FontSize"]);
        VerticalAlignmentOptions verticalAlignment = (VerticalAlignmentOptions)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["VerticalAlignment"]);
        HorizontalAlignmentOptions horizontalAlignment = (HorizontalAlignmentOptions)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["HorizontalAlignment"]);
        FontStyles fontStyle = (FontStyles)JsonSerializer.Deserialize<int, ValueTypeSerializer>(null, dict["FontStyle"]);

        cast.dataCache = new UCTextMeshProDataCache(text, fontSize, verticalAlignment, horizontalAlignment, fontStyle);

        return cast;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        UCTextMeshPro cast = (UCTextMeshPro)obj;
        JsonSerializer.AssertObject(cast.Text);
        TextMeshPro text = cast.Text;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();

        dict["Text"] = JsonSerializer.Serialize<string, ValueTypeSerializer>(text.text);
        dict["FontSize"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(text.fontSize);
        dict["VerticalAlignment"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)text.verticalAlignment);
        dict["HorizontalAlignment"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)text.horizontalAlignment);
        dict["FontStyle"] = JsonSerializer.Serialize<int, ValueTypeSerializer>((int)text.fontStyle);

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
