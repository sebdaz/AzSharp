using AzSharp.Json.Parsing;
using AzSharp.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AzSharp.Json.Serialization.TypeSerializers;

public class FlatList2DSerializer<TObject, TSerializer> : ITypeSerializer
    where TSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var dict = node.AsDict();
        int sizeX = dict["SizeX"].AsInt();
        int sizeY = dict["SizeY"].AsInt();
        List<TObject> objects = JsonSerializer.Deserialize<List<TObject>, ListSerializer<TObject, TSerializer>>(null, dict["List"])!;
        return new FlatList2D<TObject>(sizeX, sizeY, objects);
    }

    public JsonNode Serialize(object obj, Type type)
    {
        FlatList2D<TObject> list = (FlatList2D<TObject>)obj;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        dict["SizeX"] = new JsonNode(list.sizeX);
        dict["SizeY"] = new JsonNode(list.sizeY);
        dict["List"] = JsonSerializer.Serialize<List<TObject>, ListSerializer<TObject, TSerializer>>(list.list);
        return node;
    }
    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {


    }
}

public sealed class FlatList2D<TObject>
{
    public int sizeX;
    public int sizeY;
    public List<TObject> list;
    public FlatList2D()
    {
        sizeX = 0;
        sizeY = 0;
        list = new();
    }
    public FlatList2D(int sizeX, int sizeY, List<TObject> list)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.list = list;
    }
}

public class Array2DSerializer<TObject, TSerializer> : ITypeSerializer
    where TSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        FlatList2D<TObject> flatList = JsonSerializer.Deserialize<FlatList2D<TObject>, FlatList2DSerializer<TObject, TSerializer>>(null, node)!;
        TObject[,] array = new TObject[flatList.sizeX, flatList.sizeY];
        for (int x = 0; x < flatList.sizeX; x++)
        {
            for (int y = 0; y < flatList.sizeY; y++)
            {
                array[x, y] = flatList.list[x + y * flatList.sizeX];
            }
        }
        return array;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        TObject[,] array = (TObject[,])obj;
        FlatList2D<TObject> flatList = new FlatList2D<TObject>(array.GetLength(0), array.GetLength(1), new List<TObject>());
        flatList.list!.Resize(flatList.sizeX * flatList.sizeY);

        for (int x = 0; x < flatList.sizeX; x++)
        {
            for (int y = 0; y < flatList.sizeY; y++)
            {
                flatList.list[x + y * flatList.sizeX] = array[x, y];
            }
        }
        return JsonSerializer.Serialize<FlatList2D<TObject>, FlatList2DSerializer<TObject, TSerializer>>(flatList);
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}

