﻿using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Prototype;
using AzSharp.ECS.Shared.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.ECS.Shared.Entities.Prototype;

[RegisterPrototype("Entity")]
[JsonSerializable(typeof(ObjectReflectionSerializer))]
public class EntityPrototype : IPrototype
{
    [DataField("Components", typeof(EntityPrototypeComponentsSerializer))]
    public Dictionary<Type, object> Components = new();
    [DataField("Data", typeof(EntityPrototypeDataSerializer))]
    public Dictionary<Type, IEntityPrototypeData> Data = new();
}
