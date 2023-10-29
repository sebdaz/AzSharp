using AzSharp.IoC;
using AzSharp.Json.Serialization.Attributes;
using AzSharp.Json.Serialization.TypeSerializers;
using AzSharp.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzSharp.UnitTests.Setups.Prototype;

[RegisterPrototype("Test")]
[JsonSerializable(typeof(ObjectReflectionSerializer))]
internal sealed class TestPrototype : IPrototype
{
    [DataField("Int")]
    public int Int = 0;
    [DataField("String")]
    public string String = string.Empty;
}
