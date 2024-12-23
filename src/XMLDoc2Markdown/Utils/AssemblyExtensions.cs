using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XMLDoc2Markdown.Utils;

internal static class AssemblyExtensions
{
    internal static IEnumerable<string> GetDeclaredNamespaces(this Assembly assembly)
    {
        return assembly.GetTypes().Select(type => type.Namespace).Distinct();
    }

    internal static string GetAssemblyName(this Assembly assembly)
    {
        return assembly.GetName().Name;
    }
    
    internal static string GetSubNamespace(this Assembly assembly, string @namespace)
    {
        RequiredArgument.NotNull(assembly, nameof(assembly));

        var rootNamespace = assembly.GetAssemblyName();
        var subNamespace = @namespace.Replace(rootNamespace, "");
        if (string.IsNullOrWhiteSpace(subNamespace))
        {
            return null;
        }
        
        if (subNamespace.StartsWith("."))
        {
            return subNamespace.Substring(1);
        }

        return subNamespace;
    }
    
    
    internal static string GetRelativeFolderPath(this Assembly assembly, string @namespace)
    {
        RequiredArgument.NotNull(assembly, nameof(assembly));

        return assembly.GetSubNamespace(@namespace)?.Replace(".", "_").ToLower();
    }
}
