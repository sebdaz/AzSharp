using AzSharp.Info;
using UnityEngine;

namespace AzSharp.ECS.Unity.Info;

public class UnityInfoManager : IInfoManager
{
    public void PrintInfo(string info, InfoType infotype = InfoType.INFO)
    {
        string prefix;
        switch (infotype)
        {
            case InfoType.INFO:
                {
                    prefix = "INFO";
                    break;
                }
            case InfoType.WARN:
                {
                    prefix = "WARN";
                    break;
                }
            case InfoType.ERROR:
                {
                    prefix = "ERRO";
                    break;
                }
            default:
                {
                    prefix = "OTHR";
                    break;
                }
        }
        Debug.Log($"[{prefix}] {info}");
    }
}
