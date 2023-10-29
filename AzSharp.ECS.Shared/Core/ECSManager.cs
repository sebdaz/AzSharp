using AzSharp.ECS.Shared.Components;
using AzSharp.ECS.Shared.ComponentUpdates;
using AzSharp.ECS.Shared.Entities;
using AzSharp.ECS.Shared.Events;
using AzSharp.ECS.Shared.Systems;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Core;

public class ECSManager : IECSManager
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    IEntityManager ent_manager;
    IComponentManager comp_manager;
    ISystemManager system_manager;
    IEventManager event_manager;
    ICompUpdateManager comp_update_manager;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public void Prepare()
    {
        ent_manager = IoCManager.Resolve<IEntityManager>();
        comp_manager = IoCManager.Resolve<IComponentManager>();
        system_manager = IoCManager.Resolve<ISystemManager>();
        event_manager = IoCManager.Resolve<IEventManager>();
        comp_update_manager = IoCManager.Resolve<ICompUpdateManager>();
    }

    public void Tick(float delta_tick)
    {
        // In tick updates
        comp_update_manager.Update(delta_tick);
        // Post tick updates
        comp_manager.PostTick();
        ent_manager.PostTick();
    }
}
