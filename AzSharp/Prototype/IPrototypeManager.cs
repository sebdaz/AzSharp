using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;

namespace AzSharp.Prototype;

public interface IPrototypeManager
{
    public void LoadDirectory(string directory);
    public void LoadPrototypes(JsonNode node_list);
    public void LoadPrototype(JsonNode node);
    public void FinalizePrototypes();
    public void RegisterPrototype(Type prototype_type, string tag);
    public List<Prototype> GetPrototypes(Type prototype_type);
    public Prototype GetPrototype(Type prototype_type, string ID);
    public List<T> GetPrototypes<T>() where T : Prototype;
    public T GetPrototype<T>(string ID) where T : Prototype;
    public void RegisterFromAttributes();
    public void LoadString(string text);
    public void LoadFile(string path);
}
