using System.Linq;
using System.Reflection;

public static class AssemblyInfo
{
    public static string GetGitHash()
    {
        var asm = typeof(AssemblyInfo).Assembly;
        var attrs = asm.GetCustomAttributes<AssemblyMetadataAttribute>();
        return attrs.FirstOrDefault(a => a.Key == "GitHash")?.Value;
    }
}