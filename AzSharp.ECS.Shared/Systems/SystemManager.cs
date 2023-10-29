using AzSharp.IoC;
using AzSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Systems;

public class SystemManager : ISystemManager
{
    public List<ISystem> systems = new();
    public void InitializeSystems()
    {
        foreach (ISystem system in systems)
        {
            system.Initialize();
        }
    }

    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterSystemAttribute>())
        {
            RegisterSystemAttribute attribute = (RegisterSystemAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterSystemAttribute));
            RegisterSystem(type);
        }
    }

    public void RegisterSystem<T>() where T : ISystem, new()
    {
        RegisterSystem(typeof(T));
    }

    public void RegisterSystem(Type system_type)
    {
        systems.Add((ISystem)Activator.CreateInstance(system_type));
    }

    public void ShutdownSystems()
    {
        throw new NotImplementedException();
    }
}
