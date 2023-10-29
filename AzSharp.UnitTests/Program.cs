using AzSharp.Info;
using AzSharp.IoC;
using AzSharp.Prototype;
using AzSharp.Reflection;
using System;

namespace AzSharp.UnitTests;

internal class Program
{
    static void Main(string[] args)
    {
        IInfoManager info_manager = IoCManager.Register<IInfoManager, ConsoleInfoManager>();
        IReflectionManager reflection_manager = IoCManager.Register<IReflectionManager, ReflectionManager>();
        IUnitTestManager unit_test_manager = IoCManager.Register<IUnitTestManager, UnitTestManager>();
        unit_test_manager.RegisterFromAttributes();

        // Create a directory for data output for json tests
        Directory.CreateDirectory("DataOutput");

        unit_test_manager.RunUnitTests();
        Console.ReadLine();
    }
}
