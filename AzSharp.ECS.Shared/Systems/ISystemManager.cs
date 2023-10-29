using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Systems;

public interface ISystemManager
{
    public void RegisterSystem<T>()
        where T : ISystem, new();
    public void RegisterSystem(Type system_type);
    public void InitializeSystems();
    public void ShutdownSystems();
    public void RegisterFromAttributes();
}
