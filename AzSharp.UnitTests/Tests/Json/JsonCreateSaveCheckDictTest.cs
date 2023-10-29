using AzSharp.Json.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzSharp.UnitTests.Tests.Json;

[RegisterUnitTest("Json Create Save Check Dict")]
internal sealed class JsonCreateSaveCheckDictTest : UnitTest
{
    public override void Run()
    {
        const bool bool_value = true;
        const int int_value = 32;
        const float float_value = 1.37f;
        const string string_value = "string";

        JsonNode node = new JsonNode(JsonNodeType.DICTIONARY);
        var dict = node.AsDict();
        dict["Bool"] = new JsonNode(bool_value);
        dict["Int"] = new JsonNode(int_value);
        dict["Float"] = new JsonNode(float_value);
        dict["String"] = new JsonNode(string_value);

        node.SaveFile("DataOutput/json_savecheckdict.json");

        JsonNode loaded_node = new JsonNode();
        JsonError error = new JsonError();
        loaded_node.LoadFile("DataOutput/json_savecheckdict.json", error);
        if (error.Errored())
        {
            Fail(error.GetErrorMsg());
            return;
        }

        var loaded_dict = loaded_node.AsDict();
        if (loaded_dict["Bool"].AsBool() != bool_value)
        {
            Fail("Loaded bool value didn't match");
            return;
        }
        if (loaded_dict["Int"].AsInt() != int_value)
        {
            Fail("Loaded int value didn't match");
            return;
        }
        if (loaded_dict["Float"].AsFloat() != float_value)
        {
            Fail("Loaded float value didn't match");
            return;
        }
        if (loaded_dict["String"].AsString() != string_value)
        {
            Fail("Loaded string value didn't match");
            return;
        }
    }
}
