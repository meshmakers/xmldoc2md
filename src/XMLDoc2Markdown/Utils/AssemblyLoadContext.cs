using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace XMLDoc2Markdown.Utils;

internal class AssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
{
    private readonly string _pluginPath;
    private readonly AssemblyDependencyResolver _resolver;

    public AssemblyLoadContext(string pluginPath, bool isCollectible) : base(Guid.NewGuid().ToString(), isCollectible)
    {
        this._pluginPath = pluginPath;
        this._resolver = new AssemblyDependencyResolver(pluginPath);
        
        this.Resolving += this.OnResolving;
        
        
    }

    private Assembly OnResolving(System.Runtime.Loader.AssemblyLoadContext context, AssemblyName name)
    {
            string[] possiblePaths =
            [
                Path.GetDirectoryName(this._pluginPath),
                PathHelpers.GetAspNetCoreSharedPath(),
                PathHelpers.GetNetCoreSharedPath(),
                AppDomain.CurrentDomain.BaseDirectory
            ];

            foreach (string searchPath in possiblePaths)
            {
                string filePath = Path.Combine(searchPath, name.Name + ".dll");
                Console.WriteLine($"Attempting to load: {filePath}");

                if (File.Exists(filePath))
                {
                    return context.LoadFromAssemblyPath(filePath);
                }
            }
            

            Console.WriteLine($"Unable to find assembly: {name.FullName}");
            return null;
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string assemblyPath = this._resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null)
        {
            return this.LoadFromAssemblyPath(assemblyPath);
        }

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string libraryPath = this._resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null)
        {
            return this.LoadUnmanagedDllFromPath(libraryPath);
        }

        return IntPtr.Zero;
    }
}
