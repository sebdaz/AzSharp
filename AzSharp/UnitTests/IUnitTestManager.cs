using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.UnitTests;

public interface IUnitTestManager
{
    public void RunUnitTests();
    public void RegisterFromAttributes();
    public void RegisterUnitTest(Type test_type, string test_name);
    public void RegisterUnitTestSetup(Type setup_type, string setup_name);
}
