using System;
using System.IO;
using System.Runtime.InteropServices;

namespace XMLDoc2Markdown.Utils;

internal static class PathHelpers
{
    private static string GetDotNetRootPath()
    {
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),"dotnet");
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return "/usr/lib/dotnet/";
        }
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return "/usr/local/share/dotnet/";
        }

        throw new PlatformNotSupportedException("Unsupported operating system.");
    }

    internal static string GetAspNetCoreSharedPath()
    {
        string dotnetPath = GetDotNetRootPath();
        string aspNetPath = Path.Combine(dotnetPath, "shared", "Microsoft.AspNetCore.App");
        string fullPath = Path.Combine(aspNetPath, GetVersionDirectoryName(aspNetPath));
        return fullPath;
    }

    private static string GetVersionDirectoryName(string aspNetPath)
    {
        string[] versions = Directory.GetDirectories(aspNetPath, "8.0.*");
        if (versions.Length == 0)
        {
            throw new DirectoryNotFoundException("No directory found starting with 8.0.");
        }

        Array.Sort(versions);
        
        return versions[^1];
 
    }
}
