using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Core;

public interface IECSManager
{
    void Prepare();
    void Tick(float delta_tick);
}
