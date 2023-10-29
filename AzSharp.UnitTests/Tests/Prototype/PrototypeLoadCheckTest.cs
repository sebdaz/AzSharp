using AzSharp.IoC;
using AzSharp.Json.Parsing;
using AzSharp.Prototype;
using AzSharp.UnitTests.Setups.Prototype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzSharp.UnitTests.Tests.Prototype;

[RegisterUnitTest("Prototype Load Check")]
internal sealed class PrototypeLoadCheckTest : UnitTest
{
    public override void Run()
    {
        IPrototypeManager proto_manager = IoCManager.Resolve<IPrototypeManager>();
        TestPrototype parentPrototype = proto_manager.GetPrototype<TestPrototype>("TestParent");
        TestPrototype childPrototype = proto_manager.GetPrototype<TestPrototype>("TestChild");

        if (parentPrototype.Int != 5)
        {
            Fail("Parent Prototype Int is not 5");
            return;
        }
        if (parentPrototype.String != "String")
        {
            Fail("Parent Prototype String is not \"String\"");
            return;
        }

        if (childPrototype.Int != 150)
        {
            Fail("Child Prototype Int is not 150");
            return;
        }
        if (childPrototype.String != "String")
        {
            Fail("Child Prototype String is not \"String\"");
            return;
        }
    }
}
