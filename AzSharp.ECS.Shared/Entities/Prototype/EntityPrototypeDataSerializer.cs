using AzSharp.ECS.Shared.Entities;
using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public sealed class EntityPrototypeDataSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        if (node.GetNodeType() != JsonNodeType.DICTIONARY)
        {
            throw new ArgumentException("JsonNode is not dictionary during EntityPrototypeComponentsSerializer deserialization.");
        }
        Dictionary<Type, object> data_dictionary;
        if (obj == null)
        {
            data_dictionary = new();
        }
        else
        {
            data_dictionary = (Dictionary<Type, object>)obj;
        }
        IEntityManager ent_manager = IoCManager.Resolve<IEntityManager>();
        foreach (var pair in node.AsDict())
        {
            string proto_data_name = pair.Key;
            JsonNode proto_data_node = pair.Value;
            Type ent_proto_data_type = ent_manager.EntityProtoDataTypeFromName(proto_data_name);
            if (!data_dictionary.ContainsKey(ent_proto_data_type))
            {
                data_dictionary[ent_proto_data_type] = (object)Activator.CreateInstance(ent_proto_data_type);
            }
            object proto_data = data_dictionary[ent_proto_data_type];
            JsonSerializer.Deserialize(proto_data, proto_data_node, ent_proto_data_type, typeof(ObjectSerializer));
        }
        return data_dictionary;
    }

    public JsonNode Serialize(object obj, Type type)
    {
        throw new NotImplementedException();
    }

    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version)
    {
        return;
    }
}
