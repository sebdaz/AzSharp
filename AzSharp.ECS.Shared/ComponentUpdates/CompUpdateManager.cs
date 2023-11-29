using AzSharp.ECS.Shared.Components;
using AzSharp.IoC;
using AzSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.ComponentUpdates;

public class CompUpdatePair
{
    public ICompUpdateInterface comp_update_class;
    public Type comp_type;
    public CompUpdatePair(ICompUpdateInterface comp_update_class, Type comp_type)
    {
        this.comp_update_class = comp_update_class;
        this.comp_type = comp_type;
    }
}

public class CompUpdateManager : ICompUpdateManager
{
    List<CompUpdatePair> comp_update_pairs = new();
    public void RegisterCompUpdate<CompUpdate, ComponentType>()
        where CompUpdate : ICompUpdateInterface, new()
    {
        RegisterCompUpdate(typeof(CompUpdate), typeof(ComponentType));
    }

    public void RegisterCompUpdate(Type comp_update_type, Type comp_type)
    {
        comp_update_pairs.Add(new CompUpdatePair((ICompUpdateInterface)Activator.CreateInstance(comp_update_type), comp_type));
    }

    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterCompUpdateAttribute>())
        {
            RegisterCompUpdateAttribute attribute = (RegisterCompUpdateAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterCompUpdateAttribute));
            RegisterCompUpdate(type, attribute.CompType);
        }
    }

    public void Update(float delta_time)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        foreach (CompUpdatePair pair in comp_update_pairs)
        {
            ICompUpdateInterface comp_updater = pair.comp_update_class;
            List<IComponent> comp_list = comp_manager.GetAllComponents(pair.comp_type);
            foreach (IComponent comp in comp_list)
            {
                if (comp.State() != ComponentState.INITIALIZED)
                {
                    continue;
                }
                comp_updater.Update(comp, delta_time);
            }
        }
    }
}

