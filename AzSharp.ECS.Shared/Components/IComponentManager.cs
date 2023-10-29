using System;
using System.Collections.Generic;

namespace AzSharp.ECS.Shared.Components;

public interface IComponentManager
{
    public void RegisterComponent<ComponentType, ArrayType>(int priority = 0)
        where ArrayType : IComponentArray, new();
    public void RegisterComponent(Type component_type, Type array_type, Type event_raiser, int priority);
    public Component<T>? GetFirstComponent<T>();
    public Component<T> AssumeGetFirstComponent<T>();
    public uint GetFirstCompEntID<T>();
    public List<Component<T>> GetAllComponents<T>();
    public List<IComponent> GetAllComponents(Type component_type);
    public Component<T>? GetComponent<T>(uint entity_id);
    public Component<T> AssumeGetComponent<T>(uint entity_id);
    public IComponent? GetComponent(Type component_type, uint entity_id);
    public void AddComponent<T>(T component, uint entity_id);
    public void AddComponent(Type component_type, object component, uint entity_id);
    public void AddComponent<T>(uint entity_id);
    public void AddComponent(Type component_type, uint entity_id);
    public void DestroyComponent<T>(uint entity_id);
    public void DestroyComponent(Type comp_type, uint entity_id);
    public HashSet<Type>? GetEntitySignature(uint entity_id);
    public void InitializeComponent(Type component_type, uint entity_id);
    public void InitializeEntityComponents(uint entity_id);
    public void DestroyEntityComponents(uint entity_id);
    public void PostTick();
    public void CleanupComponents();
    public void RegisterFromAttributes();
    public Type ComponentTypeFromName(string component_name);
    public bool HasComponent<T>(uint entity_id);
    public bool HasComponent(Type component_type, uint entity_id);
    public List<uint> GetComponentIDs(uint entity_id);
    public string ComponentNameFromType(Type component_type);
    public IComponent AddSignedComponent(Type component_type, object component, uint entity_id, uint component_id);
    public uint GetNextComponentID();
    public List<ComponentPriority> GetComponentPriorities();
    public IComponentEventRaiser GetEventRaiser(Type comp_type);
    public void DestroyAllEntitiesWithComponent<T>();
    public Component<T> CreateEntityWithComponent<T>();
}
