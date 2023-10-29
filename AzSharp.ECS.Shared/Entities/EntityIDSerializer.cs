using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.ECS.Shared.Entities.Prototype;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities;

public class EntityIDSerializer : ITypeSerializer
{
    private Dictionary<uint, uint> realToDeferred = new();
    private Dictionary<uint, uint> deferredToReal = new();
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        int fake_id = node.AsInt();
        if (fake_id == -1)
        {
            return Entity.NULL_ENTITY;
        }
        return GetRealID((uint)fake_id);
    }

    public JsonNode Serialize(object obj, Type type)
    {
        uint real_id = (uint)obj;
        if (real_id == Entity.NULL_ENTITY)
        {
            return new JsonNode(-1);
        }
        return new JsonNode((int)GetDeferredID(real_id));
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
    public void Reset()
    {
        realToDeferred.Clear();
        deferredToReal.Clear();
    }
    public void Register(uint real_id, uint deferred_id)
    {
        realToDeferred[real_id] = deferred_id;
        deferredToReal[deferred_id] = real_id;
    }
    public uint GetRealID(uint deferred_id)
    {
        if (!deferredToReal.ContainsKey(deferred_id))
        {
            throw new InvalidOperationException("Didn't have a entity deferred to real id translation");
        }
        return deferredToReal[deferred_id];
    }
    public uint GetDeferredID(uint real_id)
    {
        if (!realToDeferred.ContainsKey(real_id))
        {
            throw new InvalidOperationException("Didn't have a entity real to deferred id translation");
        }
        return realToDeferred[real_id];
    }
}
