using System;
using System.Collections.Generic;
using System.Text;

namespace AzSharp.Info;

public sealed class ConsoleInfoManager : IInfoManager
{
    public void PrintInfo(string info, InfoType infotype = InfoType.INFO)
    {
        string prefix;
        switch (infotype)
        {
            case InfoType.INFO:
                prefix = "INFO";
                break;
            case InfoType.WARN:
                prefix = "WARN";
                break;
            case InfoType.ERROR:
                prefix = "ERRO";
                break;
            default:
                prefix = string.Empty;
                break;
        }
        Console.WriteLine($"[{prefix}] {info}");
    }
}
