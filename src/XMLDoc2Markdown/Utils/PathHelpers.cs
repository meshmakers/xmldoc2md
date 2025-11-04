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

    internal static string GetNetCoreSharedPath()
    {
        string dotnetPath = GetDotNetRootPath();
        string netCorePath = Path.Combine(dotnetPath, "shared", "Microsoft.NETCore.App");
        string fullPath = Path.Combine(netCorePath, GetVersionDirectoryName(netCorePath));
        return fullPath;
    }

    private static string GetVersionDirectoryName(string aspNetPath)
    {
        string[] versions = Directory.GetDirectories(aspNetPath, "*.*.*");
        if (versions.Length == 0)
        {
            throw new DirectoryNotFoundException("No version directory found.");
        }

        Array.Sort(versions);
        
        return versions[^1];
 
    }
}
