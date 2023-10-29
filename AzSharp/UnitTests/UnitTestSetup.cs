using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.UnitTests;

public abstract class UnitTestSetup
{
    public string name = string.Empty;
    public string failMessage = string.Empty;
    public bool failed = false;
    public void Fail(string message)
    {
        failMessage = message;
        failed = true;
    }
    public abstract void Setup();
}
