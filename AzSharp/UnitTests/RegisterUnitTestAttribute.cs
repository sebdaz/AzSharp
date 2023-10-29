using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.UnitTests;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class RegisterUnitTestAttribute : Attribute
{
    public string unitTestName;
    public RegisterUnitTestAttribute(string unitTestName)
    {
        this.unitTestName = unitTestName;
    }
}
