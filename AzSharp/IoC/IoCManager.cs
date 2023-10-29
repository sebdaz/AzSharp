using System;
using System.Collections.Generic;

namespace AzSharp.IoC;

public static class IoCManager
{
    private static Dictionary<Type, object> object_dict = new Dictionary<Type, object>();
    public static InterfaceType Register<InterfaceType, ImplementedType>()
        where InterfaceType : class
        where ImplementedType : InterfaceType, new()
    {
        Type type = typeof(InterfaceType);
        ImplementedType obj = new ImplementedType();
        if(object_dict.ContainsKey(type))
        {
            throw new ArgumentException($"IOC: Tried to register an already registered interface type of {type.Name}");
        }
        object_dict[type] = obj;
        return (InterfaceType)object_dict[type];
    }
    public static InterfaceType Resolve<InterfaceType>()
        where InterfaceType : class
    {
        Type type = typeof(InterfaceType);
        if (!object_dict.ContainsKey(type))
        {
            throw new ArgumentException($"IOC: Cannot resolve interface type of {type.Name}");
        }
        return (InterfaceType)object_dict[type];
    }
}
