using System.Collections.Generic;

namespace AzSharp.ECS.Shared.Components;

public interface IComponentArray
{
    public IComponent AddComponent(object comp, uint entity_id, uint component_id);
    public void RemoveComponent(uint entity_id);
    public IComponent? GetComponent(uint entity_id);
    public void Clear();
    public void GetAllComponents(List<IComponent> list);
    public IComponent? GetFirstComponent();
}
