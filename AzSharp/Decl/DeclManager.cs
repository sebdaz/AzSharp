using AzSharp.IoC;
using AzSharp.Prototype;
using AzSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Decl;

public sealed class DeclManager : IDeclManager
{
    private Dictionary<Type, Dictionary<string, object>> declToImplMap = new();
    public T GetDecl<T>(string tag)
    {
        Type decl_type = typeof(T);
        if (!declToImplMap.ContainsKey(decl_type))
        {
            throw new ArgumentException("Tried to get a Decl whose type is not registered.");
        }
        var dict = declToImplMap[decl_type];
        if (!dict.ContainsKey(tag))
        {
            throw new ArgumentException("Tried to get a Decl whose tag is not present.");
        }
        return (T)dict[tag];
    }

    public List<T> GetDecls<T>()
    {
        Type decl_type = typeof(T);
        if (!declToImplMap.ContainsKey(decl_type))
        {
            throw new ArgumentException("Tried to get a Decl list whose type is not registered.");
        }
        var dict = declToImplMap[decl_type];
        List<T> decls = new();
        foreach (var decl in dict.Values)
        {
            decls.Add((T)decl);
        }
        return decls;
    }

    public void RegisterDecl(Type decl_type)
    {
        if (declToImplMap.ContainsKey(decl_type))
        {
            throw new ArgumentException("Tried to register a Decl that is already registered");
        }
        declToImplMap[decl_type] = new Dictionary<string, object>();
    }

    public void RegisterDeclImpl(Type impl_type, Type decl_type, string tag)
    {
        if (!declToImplMap.ContainsKey(decl_type))
        {
            throw new ArgumentException("Tried to register a Decl Implementation to a non registered Decl");
        }
        var dict = declToImplMap[decl_type];
        if (dict.ContainsKey(tag))
        {
            throw new ArgumentException($"Tried to register a Decl Implementation with a tag that is already registered: {tag}");
        }
        dict[tag] = Activator.CreateInstance(impl_type);
    }

    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterDeclAttribute>())
        {
            RegisterDeclAttribute attribute = (RegisterDeclAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterDeclAttribute));
            RegisterDecl(type);
        }
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterDeclImplAttribute>())
        {
            RegisterDeclImplAttribute attribute = (RegisterDeclImplAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterDeclImplAttribute));
            RegisterDeclImpl(type, attribute.declType, attribute.tag);
        }
    }
}
