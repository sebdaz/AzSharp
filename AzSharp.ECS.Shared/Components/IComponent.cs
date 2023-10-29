using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Components;

public interface IComponent
{
    public uint ID();
    public uint EntityID();
    public ComponentState State();
    public object GetComponent();
    public void SetID(uint id);
    public void SetEntityID(uint ent_id);
    public void SetComponentState(ComponentState state);
    public void SetComponent(object component);

}
