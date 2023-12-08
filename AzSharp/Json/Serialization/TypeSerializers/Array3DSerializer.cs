using AzSharp.Info;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization.Attributes;
using AzSharp.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace AzSharp.Json.Serialization.TypeSerializers;


public class FlatList3DSerializer<TObject, TSerializer> : ITypeSerializer
    where TSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        var dict = node.AsDict();
        int sizeX = dict["SizeX"].AsInt();
        int sizeY = dict["SizeY"].AsInt();
        int sizeZ = dict["SizeZ"].AsInt();
        List<TObject> objects = JsonSerializer.Deserialize<List<TObject>, ListSerializer<TObject, TSerializer>>(null, dict["List"])!;
        return new FlatList3D<TObject>(sizeX, sizeY, sizeZ, objects);
    }

    public JsonNode Serialize(object obj, Type type)
    {
        FlatList3D<TObject> list = (FlatList3D<TObject>)obj;
        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        dict["SizeX"] = new JsonNode(list.sizeX);
        dict["SizeY"] = new JsonNode(list.sizeY);
        dict["SizeZ"] = new JsonNode(list.sizeZ);
        dict["List"] = JsonSerializer.Serialize<List<TObject>, ListSerializer<TObject, TSerializer>>(list.list);
        return node;
    }
    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {


    }
}

public sealed class FlatList3D<TObject>
{
    public int sizeX;
    public int sizeY;
    public int sizeZ;
    public List<TObject> list;
    public FlatList3D()
    {
        sizeX = 0;
        sizeY = 0;
        sizeZ = 0;
        list = new();
    }
    public FlatList3D(int sizeX, int sizeY, int sizeZ, List<TObject> list)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.sizeZ = sizeZ;
        this.list = list;
    }
}

public class Array3DSerializer<TObject, TSerializer> : ITypeSerializer
    where TSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        FlatList3D<TObject> flatList = JsonSerializer.Deserialize<FlatList3D<TObject>, FlatList3DSerializer<TObject, TSerializer>>(null, node)!;
        TObject[,,] array = new TObject[flatList.sizeX, flatList.sizeY, flatList.sizeZ];
        for (int x = 0; x < flatList.sizeX; x++)
        {
            for (int y = 0; y < flatList.sizeY; y++)
            {
                for (int z = 0; z < flatList.sizeZ; z++)
                {
                    array[x, y, z] = flatList.list[x + y * flatList.sizeX + z * flatList.sizeX * flatList.sizeY];
                }
            }
        }
        return array;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        TObject[,,] array = (TObject[,,])obj;
        FlatList3D<TObject> flatList = new FlatList3D<TObject>(array.GetLength(0), array.GetLength(1), array.GetLength(2), new List<TObject>());
        flatList.list!.Resize(flatList.sizeX * flatList.sizeY * flatList.sizeZ);
        
        for (int x = 0; x < flatList.sizeX; x++)
        {
            for (int y = 0; y < flatList.sizeY; y++)
            {
                for (int z = 0; z < flatList.sizeZ; z++)
                {
                    flatList.list[x + y * flatList.sizeX + z * flatList.sizeX * flatList.sizeY] = array[x, y, z];
                }
            }
        }
        return JsonSerializer.Serialize<FlatList3D<TObject>, FlatList3DSerializer<TObject, TSerializer>>(flatList);
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}

