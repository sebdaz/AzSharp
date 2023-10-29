using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace AzSharp.Reflection;

public class ReflectionManager : IReflectionManager
{
    private List<Assembly> assemblies = new();
    private  List<Type> all_type_cache = new();
    private void EnsureGetAllAssemblies()
    {
        if (assemblies.Count != 0)
        {
            return;
        }
        AppDomain current_domain = AppDomain.CurrentDomain;
        Assembly[] assems = current_domain.GetAssemblies();
        assemblies.AddRange(assems);
    }
    private void EnsureGetAllTypeCache()
    {
        if (all_type_cache.Count != 0)
        {
            return;
        }
        EnsureGetAllAssemblies();

        var type_sets = new List<Type[]>();

        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes();
            type_sets.Add(types);
        }

        foreach(var typeset in type_sets)
        {
            foreach(var type in typeset)
            {
                var attribute = (ReflectAttribute)Attribute.GetCustomAttribute(type, typeof(ReflectAttribute));
                if (attribute != null && !attribute.Discoverable)
                {
                    continue;
                }
                all_type_cache.Add(type);
            }
        }
    }
    public List<Type> GetAllChildren<T>(bool inclusive = false)
    {
        return GetAllChildren(typeof(T), inclusive);
    }
    public List<Type> GetAllChildren(Type base_type, bool inclusive = false)
    {
        List<Type> list = new();
        EnsureGetAllTypeCache();
        foreach(var type in all_type_cache)
        {
            if(type.IsAbstract || !base_type.IsAssignableFrom(type))
            {
                continue;
            }
            if (!inclusive && base_type == type)
            {
                continue;
            }
            list.Add(type);
        }
        return list;
    }
    public List<Type> FindTypesWithAttribute<T>()
        where T : Attribute
    {
        return FindTypesWithAttribute(typeof(T));
    }
    public List<Type> FindTypesWithAttribute(Type attributeType)
    {
        EnsureGetAllTypeCache();
        return all_type_cache.Where(type => Attribute.IsDefined(type, attributeType)).ToList();
    }
    public void Preload()
    {
        EnsureGetAllAssemblies();
        EnsureGetAllTypeCache();
    }

    public List<Type> GetAllTypes()
    {
        EnsureGetAllTypeCache();
        return all_type_cache;
    }
}
