using AzSharp.IoC;

namespace AzSharp.Info;

public enum InfoType : byte
{
    INFO,
    WARN,
    ERROR
}

public interface IInfoManager
{
    public void PrintInfo(string info, InfoType infotype = InfoType.INFO);
}

public static class InfoFunc
{
    public static void PrintInfo(string info, InfoType infotype = InfoType.INFO)
    {
        IInfoManager infomanager = IoCManager.Resolve<IInfoManager>();
        infomanager.PrintInfo(info, infotype);
    }
}