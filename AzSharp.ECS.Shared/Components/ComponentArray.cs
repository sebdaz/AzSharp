using System.Collections.Generic;
using System.Linq;

namespace AzSharp.ECS.Shared.Components;

public class ComponentArray<T> : IComponentArray
{
    private Dictionary<uint, Component<T>> comp_dict = new();
    public IComponent AddComponent(object comp, uint entity_id, uint component_id)
    {
        Component<T> component = new(component_id, entity_id, (T)comp);
        comp_dict[entity_id] = component;
        return component;
    }

    public void Clear()
    {
        comp_dict.Clear();
    }

    public void GetAllComponents(List<IComponent> list)
    {
        foreach(var comp in comp_dict.Values)
        {
            list.Add(comp);
        }
    }

    public IComponent? GetComponent(uint entity_id)
    {
        if (!comp_dict.ContainsKey(entity_id))
        {
            return null;
        }
        return comp_dict[entity_id];
    }

    public IComponent? GetFirstComponent()
    {
        foreach (var pair in comp_dict)
        {
            return pair.Value;
        }
        return null;
    }

    public void RemoveComponent(uint entity_id)
    {
        comp_dict.Remove(entity_id);
    }
}
