using AzSharp.ECS.Shared.Entities;
using AzSharp.ECS.Unity.GameObjectManager;
using AzSharp.ECS.Unity.Json.TypeSerializers;
using AzSharp.ECS.Unity.UnityComp;
using AzSharp.IoC;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AzSharp.ECS.Unity.Json.TypeSerializers.UnityComp;

public sealed class UCTransformRectDataCache
{
    public Vector2 pivot;
    public Vector2 anchorMax;
    public Vector2 anchorMin;
    public Vector2 sizeDelta;
    public Vector2 anchoredPosition3D;
    public UCTransformRectDataCache(Vector2 pivot, Vector2 anchorMax, Vector2 anchorMin, Vector2 sizeDelta, Vector2 anchoredPosition3D)
    {
        this.pivot = pivot;
        this.anchorMax = anchorMax;
        this.anchorMin = anchorMin;
        this.sizeDelta = sizeDelta;
        this.anchoredPosition3D = anchoredPosition3D;
    }
}

public sealed class UCTransformDataCache
{
    public string name;
    public bool active;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    public List<uint> childrenEntities;
    public UCTransformRectDataCache? rectData;
    public UCTransformDataCache(string name, bool active, Vector3 position, Quaternion rotation, Vector3 localScale, List<uint> childrenEntities, UCTransformRectDataCache? rectData)
    {
        this.name = name;
        this.active = active;
        this.position = position;
        this.rotation = rotation;
        this.localScale = localScale;
        this.childrenEntities = childrenEntities;
        this.rectData = rectData;
    }
}

public sealed class UCTransformSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        JsonSerializer.AssertObject(obj);
        UCTransform comp = (UCTransform)obj;
        if (comp == null)
        {
            throw new ArgumentException("UCTransform is null during deserialize");
        }
        var dict = node.AsDict();

        List<uint>? children = JsonSerializer.Deserialize<List<uint>, ListSerializer<uint, EntityIDSerializer>>(null, dict["Children"]);
        if (children == null)
        {
            throw new ArgumentException("Transform json node ended up not having children list");
        }

        string? name = JsonSerializer.Deserialize<string, ValueTypeSerializer>(null, dict["Name"]);
        if (name == null)
        {
            throw new ArgumentException("Transform json node ended up not having a name list");
        }
        bool active = JsonSerializer.Deserialize<bool, ValueTypeSerializer>(null, dict["Active"]);

        Vector3 position = JsonSerializer.Deserialize<Vector3, Vector3Serializer>(null, dict["Pos"]);
        Quaternion rotation = JsonSerializer.Deserialize<Quaternion, QuaternionSerializer>(null, dict["Rotation"]);
        Vector3 localScale = JsonSerializer.Deserialize<Vector3, Vector3Serializer>(null, dict["Scale"]);

        UCTransformRectDataCache? rect_data_cache = null;

        bool is_rect = JsonSerializer.Deserialize<bool, ValueTypeSerializer>(null, dict["RectTransform"]);
        if (is_rect)
        {
            JsonNode rect_data = dict["RectData"];
            var rect_dict = rect_data.AsDict();

            Vector2 pivot = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, rect_dict["Pivot"]);
            Vector2 anchorMax = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, rect_dict["AnchorMax"]);
            Vector2 anchorMin = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, rect_dict["AnchorMin"]);
            Vector2 sizeDelta = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, rect_dict["SizeDelta"]);
            Vector2 anchoredPosition3D = JsonSerializer.Deserialize<Vector2, Vector2Serializer>(null, rect_dict["AnchoredPosition3D"]);

            rect_data_cache = new(pivot, anchorMax, anchorMin, sizeDelta, anchoredPosition3D);
        }

        comp.dataCache = new UCTransformDataCache(name, active, position, rotation, localScale, children, rect_data_cache);

        return comp;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        UCTransform comp = (UCTransform)obj;
        if (comp == null)
        {
            throw new ArgumentException("UCTransform null during serialization");
        }
        GameObject? gameobject = comp.gameObject;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        if (gameobject == null)
        {
            return node;
            //throw new ArgumentException("UCTransform's gameobject was null during serialization");
        }
        Transform transf = comp.GameObject.transform;

        var dict = node.AsDict();

        dict["Name"] = JsonSerializer.Serialize<string, ValueTypeSerializer>(gameobject.name);
        dict["Active"] = JsonSerializer.Serialize<bool, ValueTypeSerializer>(gameobject.activeSelf);

        dict["Pos"] = JsonSerializer.Serialize<Vector3, Vector3Serializer>(transf.position);
        dict["Rotation"] = JsonSerializer.Serialize<Quaternion, QuaternionSerializer>(transf.rotation);
        dict["Scale"] = JsonSerializer.Serialize<Vector3, Vector3Serializer>(transf.localScale);
        dict["Children"] = JsonSerializer.Serialize<List<uint>, ListSerializer<uint, EntityIDSerializer>>(comp.GetChildrenEntities());

        RectTransform rect = gameobject.GetComponent<RectTransform>();
        if (rect)
        {
            dict["RectTransform"] = JsonSerializer.Serialize<bool, ValueTypeSerializer>(true);
            JsonNode rect_data_node = new JsonNode(JsonNodeType.DICTIONARY);
            var rect_dict = rect_data_node.AsDict();

            rect_dict["AnchoredPosition3D"] = JsonSerializer.Serialize<Vector3, Vector3Serializer>(rect.anchoredPosition3D);
            rect_dict["AnchorMax"] = JsonSerializer.Serialize<Vector2, Vector2Serializer>(rect.anchorMax);
            rect_dict["AnchorMin"] = JsonSerializer.Serialize<Vector2, Vector2Serializer>(rect.anchorMin);
            rect_dict["SizeDelta"] = JsonSerializer.Serialize<Vector2, Vector2Serializer>(rect.sizeDelta);
            rect_dict["Pivot"] = JsonSerializer.Serialize<Vector2, Vector2Serializer>(rect.pivot);

            dict["RectData"] = rect_data_node;
        }
        else
        {
            dict["RectTransform"] = JsonSerializer.Serialize<bool, ValueTypeSerializer>(false);
        }

        return node;
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
