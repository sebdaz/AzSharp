using AzSharp.IoC;
using AzSharp.Json.Parsing;
using AzSharp.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzSharp.UnitTests.Setups.Prototype;

[RegisterUnitTestSetup("Prototype Setup")]
internal sealed class PrototypeSetup : UnitTestSetup
{
    public override void Setup()
    {
        IPrototypeManager proto_manager = IoCManager.Register<IPrototypeManager, PrototypeManager>();
        proto_manager.RegisterFromAttributes();

        JsonNode node = new JsonNode();
        JsonError error = new JsonError();
        node.LoadFile("Data/prototype_load_check.json", error);
        if (error.Errored())
        {
            Fail("Couldn't load json node to load prototypes from.");
            return;
        }
        proto_manager.LoadPrototypes(node);
        proto_manager.FinalizePrototypes();
    }
}
