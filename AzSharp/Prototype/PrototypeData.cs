using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Prototype;

[JsonSerializable(typeof(ObjectReflectionSerializer))]
public class PrototypeData
{
    [DataField("Type")]
    public string Type = string.Empty;
    [DataField("ID")]
    public string ID = string.Empty;
    [DataField("Parent")]
    public string Parent = string.Empty;
    [DataField("Data", typeof(JsonNodeSerializer))]
    public JsonNode? Data = null;
}
