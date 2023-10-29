using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Json.Serialization.TypeSerializers;

public interface ITypeSerializer
{
    public JsonNode Serialize(object obj, Type type);
    public object? Deserialize(JsonNode node, object? obj, Type type, int version);
    public void VersionDataTreatment(object? obj, JsonNode node, Type type, int version);
}
