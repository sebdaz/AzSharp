using AzSharp.Json.Parsing;
using AzSharp.Utils.IDPool;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities;

public interface IEntityManager
{
    public Entity CreateEntityFromPrototype(string prototype_id, bool initialize = true);
    public Entity CreateEntity();
    public void DestroyEntity(uint entity_id);
    public Entity GetEntity(uint entity_id);
    public List<uint> GetAllEntities();
    public void InitializeEntity(uint entity_id);
    public void InitializeEntities(List<uint> entities);
    public void PostTick();
    public void CleanupEntities();
    public Entity CreatePrototype(string prototype_id, uint entity_id = Entity.NULL_ENTITY, bool initialize = true);
    public JsonNode SerializeEntity(uint entity_id);
    public JsonNode SerializeEntities(List<uint> entities);
    public JsonNode SerializeAllEntities();
    public void DeserializeEntities(JsonNode node, int version = 0);
    public void DestroyAllEntities();
    public void RegisterEntityProtoData(Type ent_proto_data_type, Type event_raiser_type);
    public void RegisterFromAttributes();
    public Type EntityProtoDataTypeFromName(string ent_proto_name);
    public IDHandle GetEntityHandle(uint ent);
    public bool ValidEntityHandle(IDHandle handle);
    public bool DestroyEntityViaHandle(IDHandle handle);
}
