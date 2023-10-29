using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Utils.IDPool;
using AzSharp.Info;
using AzSharp.IoC;
using AzSharp.ECS.Shared.Entities.Prototype;
using AzSharp.ECS.Shared.Components;
using AzSharp.Reflection;
using AzSharp.Prototype;

namespace AzSharp.ECS.Shared.Entities;

public class EntityManager : IEntityManager
{
    private Dictionary<uint, Entity> entity_dict = new();
    private IDPool ent_id_pool = new();
    private List<uint> entities_to_cleanup = new();
    private Dictionary<string, Type> entProtoDataNameToType = new();
    private Dictionary<uint, string> uninitEntPrototypes = new();
    private Dictionary<Type, IProtoDataEventRaiser> prototypeDataEventRaisers = new();
    public Entity CreateEntity()
    {
        uint id = ent_id_pool.GetNextID();
        Entity ent = new Entity(id);
        entity_dict[id] = ent;
        return ent;
    }
    public Entity GetEntity(uint entity_id)
    {
        if (!entity_dict.ContainsKey(entity_id))
        {
            throw new ArgumentException($"Tried to get a non existing entity of id {entity_id}");
        }
        return entity_dict[entity_id];
    }
    public List<uint> GetAllEntities()
    {
        return entity_dict.Keys.ToList();
    }
    public void DestroyAllEntities()
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        while (true)
        {
            foreach (var pair in entity_dict)
            {
                DestroyEntity(pair.Key);
            }
            CleanupEntities();
            comp_manager.CleanupComponents();
            if (entity_dict.Count == 0)
            {
                break;
            }
        }
    }

    public void DestroyEntity(uint entity_id)
    {
        if (!entity_dict.ContainsKey(entity_id))
        {
            InfoFunc.PrintInfo($"Tried to destroy a non existing entity of id {entity_id}", InfoType.ERROR);
            return;
        }
        Entity ent = entity_dict[entity_id];
        if (ent.State != EntityState.INITIALIZED)
        {
            // BENIGN ERROR
            //InfoFunc.PrintInfo($"Tried to destroy an entity that is not INITIALIZED; ID: {entity_id}, current state {ent.State}", InfoType.ERROR);
            return;
        }
        // Destroy the entity components aswell
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        comp_manager.DestroyEntityComponents(entity_id);

        ent.State = EntityState.DESTROYED;

        // Add to cleanup list
        entities_to_cleanup.Add(entity_id);
    }
    public void PostTick()
    {
        CleanupEntities();
    }
    public void CleanupEntities()
    {
        while (entities_to_cleanup.Count > 0)
        {
            uint entity_to_cleanup = entities_to_cleanup[entities_to_cleanup.Count - 1];
            entity_dict.Remove(entity_to_cleanup);
            entities_to_cleanup.Remove(entity_to_cleanup);
            ent_id_pool.FreeID(entity_to_cleanup);
        }
    }
    public void InitializeEntity(uint entity_id)
    {
        if (!entity_dict.ContainsKey(entity_id))
        {
            InfoFunc.PrintInfo($"Tried to initialize a non existing entity of id {entity_id}", InfoType.ERROR);
            return;
        }
        Entity ent = entity_dict[entity_id];
        if (ent.State != EntityState.UNINITIALIZED)
        {
            InfoFunc.PrintInfo($"Tried to initialize an entity that is not INITIALIZED; ID: {entity_id}, current state {ent.State}", InfoType.ERROR);
            return;
        }
        ent.State = EntityState.INITIALIZED;
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();

        comp_manager.InitializeEntityComponents(entity_id);

        ApplyProtoData(entity_id);
    }
    private void ApplyProtoData(uint entity_id)
    {
        if (!uninitEntPrototypes.ContainsKey(entity_id))
        {
            return;
        }
        string proto_name = uninitEntPrototypes[entity_id];
        IPrototypeManager proto_manager = IoCManager.Resolve<IPrototypeManager>();
        EntityPrototype ent_proto = proto_manager.GetPrototype<EntityPrototype>(proto_name);

        foreach (var data_pair in ent_proto.Data)
        {
            Type proto_data_type = data_pair.Key;
            IEntityPrototypeData proto_data = data_pair.Value;
            // Raise the event to apply this proto data
            IProtoDataEventRaiser raiser = prototypeDataEventRaisers[proto_data_type];
            raiser.RaiseApplyDataEvent(entity_id, proto_data);
        }

        uninitEntPrototypes.Remove(entity_id);
    }

    public Entity CreateEntityFromPrototype(string prototype_id)
    {
        IPrototypeManager proto_manager = IoCManager.Resolve<IPrototypeManager>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        EntityPrototype ent_proto = proto_manager.GetPrototype<EntityPrototype>(prototype_id);

        Entity ent = CreateEntity();
        foreach (var pair in ent_proto.Components)
        {
            Type comp_type = pair.Key;
            object comp_base = pair.Value;

            JsonNode comp_base_data = JsonSerializer.Serialize(comp_base, comp_type, typeof(ObjectSerializer));
            object? new_component = JsonSerializer.Deserialize(null, comp_base_data, comp_type, typeof(ObjectSerializer));
            if (new_component == null)
            {
                throw new ArgumentException("Component missing during entity prototype creation");
            }

            comp_manager.AddComponent(comp_type, new_component, ent.ID);
        }
        return ent;
    }

    public uint CreatePrototype(string prototype_id, uint entity_id = uint.MaxValue, bool initialize = true)
    {
        Entity ent;
        if (entity_id == Entity.NULL_ENTITY)
        {
            ent = CreateEntity();
        }
        else
        {
            ent = GetEntity(entity_id);
        }

        IPrototypeManager proto_manager = IoCManager.Resolve<IPrototypeManager>();
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        EntityPrototype ent_proto = proto_manager.GetPrototype<EntityPrototype>(prototype_id);

        foreach (var pair in ent_proto.Components)
        {
            Type comp_type = pair.Key;
            object comp_base = pair.Value;

            JsonNode comp_base_data = JsonSerializer.Serialize(comp_base, comp_type, typeof(ObjectSerializer));
            object? new_component = JsonSerializer.Deserialize(null, comp_base_data, comp_type, typeof(ObjectSerializer));
            if (new_component == null)
            {
                throw new ArgumentException("New component is null during prototype creation");
            }

            comp_manager.AddComponent(comp_type, new_component, ent.ID);
        }

        uninitEntPrototypes.Add(ent.ID, prototype_id);

        if (initialize)
        {
            InitializeEntity(ent.ID);
        }

        return ent.ID;
    }

    public void InitializeEntities(List<uint> entities)
    {
        foreach (uint entity in entities)
        {
            InitializeEntity(entity);
        }
    }

    public JsonNode SerializeEntity(uint entity_id)
    {
        return SerializeEntities(new List<uint> { entity_id });
    }

    public JsonNode SerializeEntities(List<uint> entities)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        EntityIDSerializer ent_id_serializer = JsonSerializer.GetSerializer<EntityIDSerializer>();
        ComponentIDSerializer comp_id_serializer = JsonSerializer.GetSerializer<ComponentIDSerializer>();
        IDPool deferred_ent_id_pool = new();
        IDPool deferred_comp_id_pool = new();

        JsonNode root_dict = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = root_dict.AsDict();
        dict["Entities"] = new JsonNode(JsonNodeType.LIST);
        dict["Components"] = new JsonNode(JsonNodeType.LIST);
        dict["ComponentData"] = new JsonNode(JsonNodeType.DICTIONARY);

        var ent_list = dict["Entities"].AsList();
        foreach (uint ent in entities)
        {
            Entity ent_instance = GetEntity(ent);
            if (ent_instance.State != EntityState.INITIALIZED)
            {
                throw new ArgumentException("Tried to serialize a non initialized entity");
            }
            uint deferred_id = deferred_ent_id_pool.GetNextID();
            ent_id_serializer.Register(ent, deferred_id);
            ent_list.Add(new JsonNode((int)deferred_id));
        }

        var comp_list = dict["Components"].AsList();
        foreach (uint ent in entities)
        {
            foreach (uint comp_id in comp_manager.GetComponentIDs(ent))
            {
                uint deferred_id = deferred_comp_id_pool.GetNextID();
                comp_id_serializer.Register(comp_id, deferred_id);
                comp_list.Add(new JsonNode((int)deferred_id));
            }
        }

        var comp_data_dict = dict["ComponentData"].AsDict();
        foreach (uint ent in entities)
        {
            HashSet<Type>? ent_sig = comp_manager.GetEntitySignature(ent);
            if (ent_sig == null)
            {
                continue;
            }
            foreach (Type comp_type in ent_sig)
            {
                string comp_name = comp_manager.ComponentNameFromType(comp_type);
                if (!comp_data_dict.ContainsKey(comp_name))
                {
                    comp_data_dict[comp_name] = new JsonNode(JsonNodeType.LIST);
                }
                var comp_container_list = comp_data_dict[comp_name].AsList();
                IComponent? comp = comp_manager.GetComponent(comp_type, ent);
                if (comp == null)
                {
                    throw new ArgumentException("Component is null during entity serialization");
                }
                if (comp.State() != ComponentState.INITIALIZED)
                {
                    throw new ArgumentException("Tried to serialize a non initialized component");
                }
                JsonNode comp_dict_node = new JsonNode(JsonNodeType.DICTIONARY);
                var comp_dict = comp_dict_node.AsDict();
                comp_dict["EntityID"] = new JsonNode((int)ent_id_serializer.GetDeferredID(ent));
                comp_dict["ID"] = new JsonNode((int)comp_id_serializer.GetDeferredID(comp.ID()));
                comp_dict["Data"] = JsonSerializer.Serialize(comp.GetComponent(), comp_type, typeof(ObjectSerializer));

                comp_container_list.Add(comp_dict_node);
            }
        }

        ent_id_serializer.Reset();
        comp_id_serializer.Reset();

        return root_dict;
    }

    public JsonNode SerializeAllEntities()
    {
        return SerializeEntities(GetAllEntities());
    }

    public void DeserializeEntities(JsonNode node, int version = 0)
    {
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        EntityIDSerializer ent_id_serializer = JsonSerializer.GetSerializer<EntityIDSerializer>();
        ComponentIDSerializer comp_id_serializer = JsonSerializer.GetSerializer<ComponentIDSerializer>();


        var root_dict = node.AsDict();

        var entity_list = root_dict["Entities"].AsList();
        var component_list = root_dict["Components"].AsList();
        var component_data = root_dict["ComponentData"].AsDict();

        foreach (JsonNode ent_node in entity_list)
        {
            uint ent_id = (uint)ent_node.AsInt();

            Entity new_ent = CreateEntity();
            new_ent.State = EntityState.INITIALIZED;

            ent_id_serializer.Register(new_ent.ID, ent_id);
        }
        foreach (JsonNode comp_node in component_list)
        {
            uint comp_id = (uint)comp_node.AsInt();

            uint new_id = comp_manager.GetNextComponentID();
            comp_id_serializer.Register(new_id, comp_id);
        }

        // Deserialize components by their priority
        foreach (ComponentPriority prio in comp_manager.GetComponentPriorities())
        {
            string comp_name = comp_manager.ComponentNameFromType(prio.componentType);
            if (component_data.ContainsKey(comp_name))
            {
                var comp_list = component_data[comp_name].AsList();

                Type comp_type = comp_manager.ComponentTypeFromName(comp_name);

                foreach (JsonNode comp_node in comp_list)
                {
                    var comp_data_dict = comp_node.AsDict();
                    object comp = Activator.CreateInstance(comp_type);
                    uint comp_id = comp_id_serializer.GetRealID((uint)comp_data_dict["ID"].AsInt());
                    uint ent_id = ent_id_serializer.GetRealID((uint)comp_data_dict["EntityID"].AsInt());
                    JsonSerializer.Deserialize(comp, comp_data_dict["Data"], comp_type, typeof(ObjectSerializer), version);

                    IComponent comp_wrapped = comp_manager.AddSignedComponent(comp_type, comp, ent_id, comp_id);
                    comp_wrapped.SetComponentState(ComponentState.INITIALIZED);
                }
            }
        }

        // Raise post deserialize event by component priority
        foreach (ComponentPriority prio in comp_manager.GetComponentPriorities())
        {
            string comp_name = comp_manager.ComponentNameFromType(prio.componentType);
            if (component_data.ContainsKey(comp_name))
            {
                var comp_list = component_data[comp_name].AsList();

                Type comp_type = comp_manager.ComponentTypeFromName(comp_name);

                foreach (JsonNode comp_node in comp_list)
                {
                    var comp_data_dict = comp_node.AsDict();
                    uint entity_id = ent_id_serializer.GetRealID((uint)comp_data_dict["EntityID"].AsInt());
                    comp_manager.GetEventRaiser(comp_type).RaisePostDeserializeEvent(entity_id);
                }
            }
        }
        // Raise late post deserialize event by component priority
        foreach (ComponentPriority prio in comp_manager.GetComponentPriorities())
        {
            string comp_name = comp_manager.ComponentNameFromType(prio.componentType);
            if (component_data.ContainsKey(comp_name))
            {
                var comp_list = component_data[comp_name].AsList();

                Type comp_type = comp_manager.ComponentTypeFromName(comp_name);

                foreach (JsonNode comp_node in comp_list)
                {
                    var comp_data_dict = comp_node.AsDict();
                    uint entity_id = ent_id_serializer.GetRealID((uint)comp_data_dict["EntityID"].AsInt());
                    comp_manager.GetEventRaiser(comp_type).RaiseLatePostDeserializeEvent(entity_id);
                }
            }
        }

        ent_id_serializer.Reset();
        comp_id_serializer.Reset();
    }

    public void RegisterEntityProtoData(Type ent_proto_data_type, Type event_raiser_type)
    {
        string name = ent_proto_data_type.Name;
        entProtoDataNameToType[name] = ent_proto_data_type;
        prototypeDataEventRaisers[ent_proto_data_type] = (IProtoDataEventRaiser)Activator.CreateInstance(event_raiser_type);
    }

    public void RegisterFromAttributes()
    {
        foreach (var comp_type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterEntityPrototypeDataAttribute>())
        {
            RegisterEntityPrototypeDataAttribute comp_attribute = (RegisterEntityPrototypeDataAttribute)Attribute.GetCustomAttribute(comp_type, typeof(RegisterEntityPrototypeDataAttribute));
            RegisterEntityProtoData(comp_type, comp_attribute.EventRaiserType);
        }
    }

    public Type EntityProtoDataTypeFromName(string ent_proto_name)
    {
        if (!entProtoDataNameToType.ContainsKey(ent_proto_name))
        {
            throw new ArgumentException($"Cant find a matching type for ent proto data name: {ent_proto_name}");
        }
        return entProtoDataNameToType[ent_proto_name];
    }
}
