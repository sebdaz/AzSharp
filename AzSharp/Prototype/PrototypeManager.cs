using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Info;
using AzSharp.IoC;
using AzSharp.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AzSharp.Prototype;

public class PrototypeManager : IPrototypeManager
{
    private Dictionary<Type, PrototypeArray> ProtoDict = new();
    private Dictionary<string, Type> TagDict = new();
    public void FinalizePrototypes()
    {
        foreach (var pair in ProtoDict)
        {
            pair.Value.FinalizePrototypes();
        }
    }

    public IPrototype GetPrototype(Type prototype_type, string ID)
    {
        if (!ProtoDict.ContainsKey(prototype_type))
        {
            throw new ArgumentException($"Tried to get a non existing prototype type: {prototype_type}");
        }
        var array = ProtoDict[prototype_type];
        if (!array.PrototypeDict.ContainsKey(ID))
        {
            throw new ArgumentException($"Tried to get a non existing prototype ID: {ID}");
        }
        return array.PrototypeDict[ID];
    }

    public T GetPrototype<T>(string ID) where T : IPrototype
    {
        return (T)GetPrototype(typeof(T), ID);
    }

    public List<IPrototype> GetPrototypes(Type prototype_type)
    {
        throw new NotImplementedException();
    }

    public List<T> GetPrototypes<T>() where T : IPrototype
    {
        throw new NotImplementedException();
    }

    public void LoadDirectory(string directory)
    {
        string[] files = Directory.GetFiles(directory, "*.json", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            LoadFile(file);
        }
    }
    public void LoadString(string text)
    {
        JsonError error = new();
        JsonNode node = new();
        node.LoadText(text, error);
        if (error.Errored())
        {
            InfoFunc.PrintInfo($"Errored during LoadString of Prototypes for file {text}. {error.GetErrorMsg()}", InfoType.ERROR);
            return;
        }
        if (node.GetNodeType() != JsonNodeType.LIST)
        {
            InfoFunc.PrintInfo($"Loaded a non-list node during LoadString of Prototypes for file {text}", InfoType.ERROR);
            return;
        }
        LoadPrototypes(node);
    }
    public void LoadFile(string path)
    {
        JsonError error = new();
        JsonNode node = new();
        node.LoadFile(path, error);
        if (error.Errored())
        {
            InfoFunc.PrintInfo($"Errored during LoadFile of Prototypes for file {path}. {error.GetErrorMsg()}", InfoType.ERROR);
            return;
        }
        if (node.GetNodeType() != JsonNodeType.LIST)
        {
            InfoFunc.PrintInfo($"Loaded a non-list node during LoadFile of Prototypes for file {path}", InfoType.ERROR);
            return;
        }
        LoadPrototypes(node);
    }

    public void LoadPrototype(JsonNode node)
    {
        PrototypeData? protodata = JsonSerializer.Deserialize<PrototypeData, ObjectSerializer>(null, node);
        if (protodata == null)
        {
            throw new ArgumentException("Protodata was null");
        }
        if (protodata.Data == null)
        {
            throw new ArgumentException("Protodata data is null");
        }
        PrototypeArray array = ProtoDict[TagDict[protodata.Type]];
        array.LoadPrototypeData(protodata);
    }

    public void LoadPrototypes(JsonNode node_list)
    {
        if (node_list.GetNodeType() != JsonNodeType.LIST)
        {
            InfoFunc.PrintInfo($"Tried to load prototypes from a non-list node", InfoType.ERROR);
            return;
        }
        foreach (var node in node_list.AsList())
        {
            if (node.GetNodeType() != JsonNodeType.DICTIONARY)
            {
                InfoFunc.PrintInfo($"Loaded a non-dictionary node during LoadPrototypes", InfoType.ERROR);
                continue;
            }
            LoadPrototype(node);
        }
    }

    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterPrototypeAttribute>())
        {
            RegisterPrototypeAttribute attribute = (RegisterPrototypeAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterPrototypeAttribute));
            RegisterPrototype(type, attribute.Tag);
        }
    }

    public void RegisterPrototype(Type prototype_type, string tag)
    {
        if (TagDict.ContainsKey(tag))
        {
            InfoFunc.PrintInfo($"Tried to register prototype with a tag that's already registered of {tag}", InfoType.WARN);
            return;
        }
        if (ProtoDict.ContainsKey(prototype_type))
        {
            InfoFunc.PrintInfo($"Tried to register prototype with a type that's already registered of {prototype_type}", InfoType.WARN);
            return;
        }
        TagDict[tag] = prototype_type;
        ProtoDict[prototype_type] = new PrototypeArray(prototype_type);
    }
}
