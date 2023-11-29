using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzSharp.Prototype;

public class PrototypeArray : IPrototypeArray
{
    private Type PrototypeType;
    public Dictionary<string, Prototype> PrototypeDict = new();
    public Dictionary<string, PrototypeData> ProtoDataDict = new();
    public PrototypeArray(Type prototype)
    {
        PrototypeType = prototype;
    }
    public void LoadPrototypeData(PrototypeData data)
    {
        ProtoDataDict[data.ID] = data;
    }
    public void FinalizePrototypes()
    {
        foreach (var pair in ProtoDataDict)
        {
            FinalizePrototype(pair.Key);
        }
    }
    private void FinalizePrototype(string ID)
    {
        PrototypeData data = ProtoDataDict[ID];
        if (data.Parent != string.Empty && !PrototypeDict.ContainsKey(data.Parent))
        {
            FinalizePrototype(data.Parent);
        }
        Prototype prototype = (Prototype)Activator.CreateInstance(PrototypeType);
        ApplyProtoData(data, prototype);
        prototype.ID = ID;

        PrototypeDict[ID] = prototype;
    }
    private void ApplyProtoData(PrototypeData data, Prototype prototype)
    {
        if (data.Parent != string.Empty)
        {
            PrototypeData parent_data = ProtoDataDict[data.Parent];
            ApplyProtoData(parent_data, prototype);
        }
        if (data.Data == null)
        {
            throw new ArgumentException("data.Data is null");
        }
        JsonSerializer.Deserialize(prototype, data.Data, PrototypeType, typeof(ObjectSerializer));
    }
    
}
