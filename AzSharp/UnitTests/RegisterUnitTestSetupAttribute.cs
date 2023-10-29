using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.UnitTests;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterUnitTestSetupAttribute : Attribute
{
    public string unitTestSetupName;
    public RegisterUnitTestSetupAttribute(string unitTestSetupName)
    {
        this.unitTestSetupName = unitTestSetupName;
    }
}
