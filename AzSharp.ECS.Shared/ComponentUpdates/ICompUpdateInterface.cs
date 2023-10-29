using AzSharp.ECS.Shared.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.ComponentUpdates;

public interface ICompUpdateInterface
{
    public void Update(IComponent component, float delta_time);
}
