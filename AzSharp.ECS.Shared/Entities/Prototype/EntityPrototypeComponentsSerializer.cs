using AzSharp.Json.Parsing;
using AzSharp.Json.Serialization;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Prototype;
using AzSharp.IoC;
using System;
using System.Collections.Generic;
using System.Text;
using AzSharp.ECS.Shared.Components;

namespace AzSharp.ECS.Shared.Entities.Prototype;

public class EntityPrototypeComponentsSerializer : ITypeSerializer
{
    public object? Deserialize(JsonNode node, object? obj, Type type, int version)
    {
        if (node.GetNodeType() != JsonNodeType.DICTIONARY)
        {
            throw new ArgumentException("JsonNode is not dictionary during EntityPrototypeComponentsSerializer deserialization.");
        }

        Dictionary<Type, object> comp_dictionary;
        if (obj == null)
        {
            comp_dictionary = new();
        }
        else
        {
            comp_dictionary = (Dictionary<Type, object>)obj;
        }
        IComponentManager comp_manager = IoCManager.Resolve<IComponentManager>();
        foreach (var pair in node.AsDict())
        {
            string component_name = pair.Key;
            JsonNode comp_node = pair.Value;
            Type component_type = comp_manager.ComponentTypeFromName(component_name);
            if (!comp_dictionary.ContainsKey(component_type))
            {
                comp_dictionary[component_type] = Activator.CreateInstance(component_type);
            }
            object comp = comp_dictionary[component_type];
            JsonSerializer.Deserialize(comp, comp_node, component_type, typeof(ObjectSerializer));
        }
        return comp_dictionary;
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
