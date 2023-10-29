using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AzSharp.ECS.Unity.Json.TypeSerializers;

public sealed class ColorBlockSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var dict = node.AsDict();
        ColorBlock block = new();

        block.normalColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["NormalColor"]);
        block.highlightedColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["HighlightedColor"]);
        block.pressedColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["PressedColor"]);
        block.selectedColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["SelectedColor"]);
        block.disabledColor = JsonSerializer.Deserialize<Color, ColorSerializer>(null, dict["DisabledColor"]);
        block.colorMultiplier = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["ColorMultiplier"]);
        block.fadeDuration = JsonSerializer.Deserialize<float, ValueTypeSerializer>(null, dict["FadeDuration"]);

        return block;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        ColorBlock cast = (ColorBlock)obj;
        JsonNode dict_node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = dict_node.AsDict();

        dict["NormalColor"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.normalColor);
        dict["HighlightedColor"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.highlightedColor);
        dict["PressedColor"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.pressedColor);
        dict["SelectedColor"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.selectedColor);
        dict["DisabledColor"] = JsonSerializer.Serialize<Color, ColorSerializer>(cast.disabledColor);
        dict["ColorMultiplier"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(cast.colorMultiplier);
        dict["FadeDuration"] = JsonSerializer.Serialize<float, ValueTypeSerializer>(cast.fadeDuration);

        return dict_node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
