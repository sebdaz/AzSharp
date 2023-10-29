using AzSharp.Info;
using System;
using System.Collections.Generic;
using AzSharp.IoC;
using AzSharp.Utils.IDPool;
using AzSharp.Reflection;
using System.Linq;
using AzSharp.ECS.Shared.Entities;

namespace AzSharp.ECS.Shared.Components;

public class ComponentManager : IComponentManager
{
    private Dictionary<Type, List<uint>> cleanup_comp_to_ent_list = new();
    private Dictionary<Type, IComponentEventRaiser> event_raisers = new();
    private Dictionary<Type, IComponentArray> array_dicts = new();
    private IDPool component_id_pool = new();
    private Dictionary<uint, HashSet<Type>> entity_component_signatures = new();
    private Dictionary<string, Type> componentNameToType = new();
    private Dictionary<Type, string> componentTypeToName = new();
    private List<ComponentPriority> componentPriorities = new();
    private void AddComponentPriority(Type componentType, int priority)
    {
        componentPriorities.Add(new ComponentPriority(componentType, priority));
        componentPriorities = componentPriorities.OrderByDescending(o => o.priority).ToList();
    }
    private void AddCleanupCompTypeEnt(Type comp_type, uint entity_id)
    {
        if (!cleanup_comp_to_ent_list.ContainsKey(comp_type))
        {
            cleanup_comp_to_ent_list[comp_type] = new List<uint>();
        }
        cleanup_comp_to_ent_list[comp_type].Add(entity_id);
    }
    private void RemoveCleanupCompTypeEnt(Type comp_type, uint entity_id)
    {
        List<uint> list = cleanup_comp_to_ent_list[comp_type];
        list.Remove(entity_id);
        IComponentArray array = GetComponentArray(comp_type);
        IComponent? comp = array.GetComponent(entity_id);
        if (comp == null)
        {
            throw new ArgumentException("Component not found during cleanup");
        }
        component_id_pool.FreeID(comp.ID());
        array.RemoveComponent(entity_id);
        RemoveSignature(comp_type, entity_id);

        if (list.Count == 0)
        {
            cleanup_comp_to_ent_list.Remove(comp_type);
        }
    }
    public IComponentEventRaiser GetEventRaiser(Type comp_type)
    {
        return event_raisers[comp_type];
    }
    public void InitializeComponent(Type component_type, uint entity_id)
    {
        IComponentArray array = GetComponentArray(component_type);
        IComponent? comp = array.GetComponent(entity_id);
        if (comp == null)
        {
            throw new ArgumentException("Component not found during initialization");
        }
        comp.SetComponentState(ComponentState.INITIALIZED);
        IComponentEventRaiser raiser = GetEventRaiser(component_type);
        raiser.RaiseInitEvent(entity_id);
    }
    private void AddSignature(Type comp_type, uint entity_id)
    {
        if (!entity_component_signatures.ContainsKey(entity_id))
        {
            entity_component_signatures[entity_id] = new HashSet<Type>();
        }
        entity_component_signatures[entity_id].Add(comp_type);
    }
    private void RemoveSignature(Type comp_type, uint entity_id)
    {
        entity_component_signatures[entity_id].Remove(comp_type);
        if (entity_component_signatures[entity_id].Count == 0)
        {
            entity_component_signatures.Remove(entity_id);
        }
    }
    public HashSet<Type>? GetEntitySignature(uint entity_id)
    {
        if (!entity_component_signatures.ContainsKey(entity_id))
        {
            return null;
        }
        return entity_component_signatures[entity_id];
    }
    private IComponentArray GetComponentArray(Type comp_type)
    {
        return array_dicts[comp_type];
    }

    public List<Component<T>> GetAllComponents<T>()
    {
        IComponentArray comp_array = GetComponentArray(typeof(T));
        List<Component<T>> comp_list = new();
        List<IComponent> list = new();
        comp_array.GetAllComponents(list);
        foreach (IComponent component in list)
        {
            comp_list.Add((Component<T>)component);
        }
        return comp_list;
    }

    public Component<T>? GetFirstComponent<T>()
    {
        IComponentArray comp_array = GetComponentArray(typeof(T));
        return (Component<T>?)comp_array.GetFirstComponent();
    }

    public void RegisterComponent<ComponentType, ArrayType>(int priority = 0)
        where ArrayType : IComponentArray, new()
    {
        RegisterComponent(typeof(ComponentType), typeof(ArrayType), typeof(ComponentEventRaiser<ComponentType>), priority);
    }

    public Component<T>? GetComponent<T>(uint entity_id)
    {
        return (Component<T>?)GetComponent(typeof(T), entity_id);
    }

    public void DestroyComponent<T>(uint entity_id)
    {
        DestroyComponent(typeof(T), entity_id);
    }
    public void DestroyComponent(Type comp_type, uint entity_id)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        if (ent_manager == null)
        {
            InfoFunc.PrintInfo("Not found entity manager", InfoType.ERROR);
            return;
        }
        Entity ent = ent_manager.GetEntity(entity_id);
        if (ent == null)
        {
            InfoFunc.PrintInfo("Tried to destroy a component from a non-existing entity", InfoType.ERROR);
            return;
        }
        if (ent.State == EntityState.DESTROYED)
        {
            InfoFunc.PrintInfo("Tried to destroy a component from a destroyed entity", InfoType.ERROR);
            return;
        }
        IComponentArray comp_array = GetComponentArray(comp_type);
        if (comp_array.GetComponent(entity_id) == null)
        {
            InfoFunc.PrintInfo("Tried to destroy a non existing component from an entity", InfoType.ERROR);
            return;
        }
        IComponent? comp = comp_array.GetComponent(entity_id);
        if (comp == null)
        {
            throw new ArgumentException("Component missing during destruction");
        }
        comp.SetComponentState(ComponentState.DESTROYED);
        IComponentEventRaiser raiser = GetEventRaiser(comp_type);
        raiser.RaiseDestroyEvent(entity_id);

        AddCleanupCompTypeEnt(comp_type, entity_id);
    }
    public void PostTick()
    {
        CleanupComponents();
    }
    public void CleanupComponents()
    {
        while (cleanup_comp_to_ent_list.Count > 0)
        {
            var pair = cleanup_comp_to_ent_list.First();
            Type comp_type = pair.Key;
            List<uint> ent_list = pair.Value;
            while (ent_list.Count > 0)
            {
                uint ent = ent_list[ent_list.Count - 1];
                RemoveCleanupCompTypeEnt(comp_type, ent);
            }

        }
    }
    public List<IComponent> GetAllComponents(Type component_type)
    {
        IComponentArray comp_array = GetComponentArray(component_type);
        List<IComponent> list = new();
        comp_array.GetAllComponents(list);
        return list;
    }

    public void RegisterFromAttributes()
    {
        foreach (var comp_type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterComponentAttribute>())
        {
            RegisterComponentAttribute comp_attribute = (RegisterComponentAttribute)Attribute.GetCustomAttribute(comp_type, typeof(RegisterComponentAttribute));
            RegisterComponent(comp_type, comp_attribute.ContainerType, comp_attribute.EventRaiserType, comp_attribute.InitPriority);
        }
    }

    public void RegisterComponent(Type component_type, Type array_type, Type event_raiser, int priority)
    {
        if (array_dicts.ContainsKey(component_type))
        {
            InfoFunc.PrintInfo($"Tried to register an already registered component of type {component_type.Name}", InfoType.WARN);
            return;
        }
        AddComponentPriority(component_type, priority);
        componentNameToType[component_type.Name] = component_type;
        componentTypeToName[component_type] = component_type.Name;
        array_dicts[component_type] = (IComponentArray)Activator.CreateInstance(array_type);
        event_raisers[component_type] = (IComponentEventRaiser)Activator.CreateInstance(event_raiser);
    }

    public Type ComponentTypeFromName(string component_name)
    {
        return componentNameToType[component_name];
    }
    public string ComponentNameFromType(Type component_type)
    {
        return componentTypeToName[component_type];
    }

    public uint GetFirstCompEntID<T>()
    {
        Component<T>? first_comp = GetFirstComponent<T>();
        if (first_comp == null)
        {
            return Entity.NULL_ENTITY;
        }
        return first_comp.entityID;
    }

    public bool HasComponent<T>(uint entity_id)
    {
        return HasComponent(typeof(T), entity_id);
    }

    public bool HasComponent(Type component_type, uint entity_id)
    {
        IComponent? comp = GetComponent(component_type, entity_id);
        if (comp == null)
        {
            return false;
        }
        return true;
    }

    public IComponent? GetComponent(Type component_type, uint entity_id)
    {
        IComponentArray comp_array = GetComponentArray(component_type);
        return comp_array.GetComponent(entity_id);
    }
    public void AddComponent<T>(T component, uint entity_id)
    {
#pragma warning disable CS8604 // Possible null reference argument.
        AddComponent(typeof(T), component, entity_id);
#pragma warning restore CS8604 // Possible null reference argument.
    }
    public void AddComponent<T>(uint entity_id)
    {
        Type comp_type = typeof(T);
        AddComponent(comp_type, Activator.CreateInstance(comp_type), entity_id);
    }

    public void AddComponent(Type component_type, uint entity_id)
    {
        AddComponent(component_type, Activator.CreateInstance(component_type), entity_id);
    }
    public void AddComponent(Type component_type, object component, uint entity_id)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        if (ent_manager == null)
        {
            InfoFunc.PrintInfo("Not found entity manager", InfoType.ERROR);
            return;
        }
        Entity ent = ent_manager.GetEntity(entity_id);
        if (ent == null)
        {
            InfoFunc.PrintInfo($"Tried to add a component of type {component_type} to a non-existing entity", InfoType.ERROR);
            return;
        }
        if (ent.State == EntityState.DESTROYED)
        {
            InfoFunc.PrintInfo($"Tried to add a component of type {component_type} to a destroyed entity", InfoType.ERROR);
            return;
        }
        IComponentArray comp_array = GetComponentArray(component_type);
        if (comp_array.GetComponent(entity_id) != null)
        {
            InfoFunc.PrintInfo($"Tried to add a component of type {component_type} to an entity that already has it", InfoType.ERROR);
            return;
        }
        // Add signature
        AddSignature(component_type, entity_id);
        // Set the entity ID and component ID
        uint next_comp_id = component_id_pool.GetNextID();
        // Insert into array
        IComponent component_wrap = comp_array.AddComponent(component, entity_id, next_comp_id);
        // Attach component
        component_wrap.SetComponentState(ComponentState.ATTACHED);
        IComponentEventRaiser raiser = GetEventRaiser(component_type);
        raiser.RaiseAttachEvent(ent.ID);
        //If entity is initialized, then initialize the component aswell
        if (ent.State == EntityState.INITIALIZED)
        {
            component_wrap.SetComponentState(ComponentState.INITIALIZED);
            raiser.RaiseInitEvent(ent.ID);
        }
    }

    public void InitializeEntityComponents(uint entity_id)
    {
        HashSet<Type>? entity_sig = GetEntitySignature(entity_id);
        if (entity_sig == null)
        {
            return;
        }
        foreach (ComponentPriority comp_prio in componentPriorities)
        {
            Type comp_type = comp_prio.componentType;
            if (entity_sig.Contains(comp_type))
            {
                InitializeComponent(comp_type, entity_id);
            }
        }
    }
    public void DestroyEntityComponents(uint entity_id)
    {
        HashSet<Type>? entity_sig = GetEntitySignature(entity_id);
        if (entity_sig == null)
        {
            return;
        }
        for (int i = componentPriorities.Count - 1; i >= 0; i--)
        {
            ComponentPriority comp_prio = componentPriorities[i];
            Type comp_type = comp_prio.componentType;
            if (entity_sig.Contains(comp_type))
            {
                DestroyComponent(comp_type, entity_id);
            }
        }
    }

    public List<uint> GetComponentIDs(uint entity_id)
    {
        List<uint> id_list = new();
        HashSet<Type>? entity_sig = GetEntitySignature(entity_id);
        if (entity_sig == null)
        {
            return id_list;
        }
        foreach (Type comp_type in entity_sig)
        {
            IComponent? comp = GetComponent(comp_type, entity_id);
            if (comp == null)
            {
                throw new ArgumentException("Component is null during getting component IDs");
            }
            id_list.Add(comp.ID());
        }
        return id_list;
    }

    public IComponent AddSignedComponent(Type component_type, object component, uint entity_id, uint component_id)
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        if (ent_manager == null)
        {
            InfoFunc.PrintInfo("Not found entity manager", InfoType.ERROR);
            throw new ArgumentException("");
        }
        Entity ent = ent_manager.GetEntity(entity_id);
        if (ent == null)
        {
            InfoFunc.PrintInfo($"Tried to add a component to a non-existing entity, component type: {component_type} ; entity id {entity_id}", InfoType.ERROR);
            throw new ArgumentException("");
        }
        if (ent.State == EntityState.DESTROYED)
        {
            InfoFunc.PrintInfo($"Tried to add a component to a destroyed entity, component type: {component_type} ; entity id {entity_id}", InfoType.ERROR);
            throw new ArgumentException("");
        }
        IComponentArray comp_array = GetComponentArray(component_type);
        if (comp_array.GetComponent(entity_id) != null)
        {
            InfoFunc.PrintInfo($"Tried to add a component to an entity that already has it, component type: {component_type} ; entity id {entity_id}", InfoType.ERROR);
            throw new ArgumentException("");
        }
        // Add signature
        AddSignature(component_type, entity_id);
        return comp_array.AddComponent(component, entity_id, component_id);
    }

    public uint GetNextComponentID()
    {
        return component_id_pool.GetNextID();
    }
    public List<ComponentPriority> GetComponentPriorities()
    {
        return componentPriorities;
    }

    public Component<T> AssumeGetComponent<T>(uint entity_id)
    {
        Component<T>? comp = GetComponent<T>(entity_id);
        if (comp == null)
        {
            throw new InvalidOperationException("Tried to assume get a non existing component");
        }
        return comp;
    }

    public void DestroyAllEntitiesWithComponent<T>()
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        
        foreach(Component<T> comp in GetAllComponents<T>())
        {
            ent_manager.DestroyEntity(comp.entityID);
        }
    }

    public Component<T> AssumeGetFirstComponent<T>()
    {
        Component<T>? comp = GetFirstComponent<T>();
        if (comp == null)
        {
            throw new ArgumentException("Tried to assume get first component which didn't exit");
        }
        return comp;
    }

    public Component<T> CreateEntityWithComponent<T>()
    {
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        Entity ent = ent_manager.CreateEntity();
        AddComponent<T>(ent.ID);
        Component<T> comp = AssumeGetComponent<T>(ent.ID);
        ent_manager.InitializeEntity(ent.ID);
        return comp;
    }
}
