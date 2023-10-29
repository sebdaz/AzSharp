using AzSharp.Info;
using AzSharp.IoC;
using AzSharp.Prototype;
using AzSharp.Reflection;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace AzSharp.UnitTests;

public sealed class UnitTestManager : IUnitTestManager
{
    private List<UnitTestSetup> setupsList = new();
    private List<UnitTest> unitTests = new();
    public void RegisterFromAttributes()
    {
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterUnitTestAttribute>())
        {
            RegisterUnitTestAttribute attribute = (RegisterUnitTestAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterUnitTestAttribute));
            RegisterUnitTest(type, attribute.unitTestName);
        }
        foreach (var type in IoCManager.Resolve<IReflectionManager>().FindTypesWithAttribute<RegisterUnitTestSetupAttribute>())
        {
            RegisterUnitTestSetupAttribute attribute = (RegisterUnitTestSetupAttribute)Attribute.GetCustomAttribute(type, typeof(RegisterUnitTestSetupAttribute));
            RegisterUnitTestSetup(type, attribute.unitTestSetupName);
        }
    }

    public void RegisterUnitTest(Type test_type, string test_name)
    {
        UnitTest test = (UnitTest)Activator.CreateInstance(test_type);
        test.name = test_name;
        unitTests.Add(test);
    }

    public void RegisterUnitTestSetup(Type setup_type, string setup_name)
    {
        UnitTestSetup test = (UnitTestSetup)Activator.CreateInstance(setup_type);
        test.name = setup_name;
        setupsList.Add(test);
    }

    public void RunUnitTests()
    {
        if (!RunSetups())
        {
            return;
        }
        if (!RunTests())
        {
            return;
        }
    }
    private bool RunSetups()
    {
        IInfoManager info_manager = IoCManager.Resolve<IInfoManager>();
        int setups_count = 0;
        int passed_setups_count = 0;
        info_manager.PrintInfo($"Running unit test setups - {setupsList.Count} setups to run...", InfoType.INFO);
        foreach (var test in setupsList)
        {
            setups_count++;
            try
            {
                test.Setup();
                if (test.failed)
                {
                    info_manager.PrintInfo($"{setups_count}. {test.name} - FAILED: {test.failMessage}.", InfoType.WARN);
                }
                else
                {
                    info_manager.PrintInfo($"{setups_count}. {test.name} - PASSED.", InfoType.INFO);
                    passed_setups_count++;
                }
            }
            catch (Exception ex)
            {
                info_manager.PrintInfo($"{setups_count}. {test.name} - EXCEPTION: {ex.Message}", InfoType.ERROR);
            }
        }
        info_manager.PrintInfo($"Completed setups - {passed_setups_count}/{setupsList.Count} setups passed.", InfoType.INFO);
        if (passed_setups_count != setupsList.Count)
        {
            info_manager.PrintInfo($"Warning! - {setupsList.Count - passed_setups_count} setup(s) not passed!", InfoType.WARN);
            return false;
        }
        return true;
    }
    private bool RunTests()
    {
        IInfoManager info_manager = IoCManager.Resolve<IInfoManager>();
        int unit_test_count = 0;
        int passed_test_count = 0;
        info_manager.PrintInfo($"Running unit tests - {unitTests.Count} tests to run...", InfoType.INFO);
        foreach (var test in unitTests)
        {
            unit_test_count++;
            try
            {
                test.Run();
                if (test.failed)
                {
                    info_manager.PrintInfo($"{unit_test_count}. {test.name} - FAILED: {test.failMessage}.", InfoType.WARN);
                }
                else
                {
                    info_manager.PrintInfo($"{unit_test_count}. {test.name} - PASSED.", InfoType.INFO);
                    passed_test_count++;
                }
            }
            catch (Exception ex)
            {
                info_manager.PrintInfo($"{unit_test_count}. {test.name} - EXCEPTION: {ex.Message}", InfoType.ERROR);
            }
        }
        info_manager.PrintInfo($"Completed unit tests - {passed_test_count}/{unitTests.Count} tests passed.", InfoType.INFO);
        if (passed_test_count != unitTests.Count)
        {
            info_manager.PrintInfo($"Warning! - {unitTests.Count - passed_test_count} unit test(s) not passed!", InfoType.WARN);
        }
        return true;
    }
}
