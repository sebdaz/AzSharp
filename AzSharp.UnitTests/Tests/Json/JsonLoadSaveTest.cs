using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzSharp.UnitTests.Tests.Json;

[RegisterUnitTest("Json Load Save")]
internal sealed class JsonLoadSaveTest : UnitTest
{
    public override void Run()
    {
        JsonNode node = new JsonNode();
        JsonError error = new JsonError();
        node.LoadFile("Data/json_load_save.json", error);
        if (error.Errored())
        {
            Fail(error.GetErrorMsg());
            return;
        }
        node.SaveFile("DataOutput/json_load_save.json");
    }
}
