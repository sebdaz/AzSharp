using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Systems;

public interface ISystem
{
    public void Initialize();
    public void Shutdown();
}
