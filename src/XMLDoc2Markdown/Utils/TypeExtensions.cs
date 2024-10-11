using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.JavaScript;
using System.Text.RegularExpressions;
using Markdown;

namespace XMLDoc2Markdown.Utils;

internal static class TypeExtensions
{
    internal static readonly IReadOnlyDictionary<Type, string> simplifiedTypeNames = new Dictionary<Type, string>
    {
        // void
        { typeof(void), "void" },
        // object
        { typeof(object), "object" },
        // boolean
        { typeof(bool), "bool" },
        // numeric
        { typeof(sbyte), "sbyte" },
        { typeof(byte), "byte" },
        { typeof(short), "short" },
        { typeof(ushort), "ushort" },
        { typeof(int), "int" },
        { typeof(uint), "uint" },
        { typeof(long), "long" },
        { typeof(ulong), "ulong" },
        { typeof(float), "float" },
        { typeof(double), "double" },
        { typeof(decimal), "decimal" },
        // text
        { typeof(char), "char" },
        { typeof(string), "string" },
    };

    internal static string GetSimplifiedName(this Type type)
    {
        return simplifiedTypeNames.TryGetValue(type, out string simplifiedName) ? simplifiedName : type.Name;
    }

    internal static Visibility GetVisibility(this Type type)
    {
        if (type.IsPublic)
        {
            return Visibility.Public;
        }
        else
        {
            return Visibility.None;
        }
    }

    public static bool IsRecordType(this Type type)
    {
        // Check if the type is a class since records are class types
        if (!type.IsClass)
            return false;

        // Look for the EqualityContract property, which is unique to records
        PropertyInfo equalityContractProperty = type.GetProperty("EqualityContract", BindingFlags.NonPublic | BindingFlags.Instance);
        return equalityContractProperty != null;
    }

    internal static string GetSignature(this Type type, bool full = false)
    {
        List<string> signature = new();

        if (full)
        {
            signature.Add(type.GetVisibility().Print());

            if (type.IsClass)
            {
                switch (type.IsAbstract)
                {
                    case true when type.IsSealed:
                        signature.Add("static");
                        break;
                    case true:
                        signature.Add("abstract");
                        break;
                    default:
                    {
                        if (type.IsSealed && !type.IsRecordType())
                        {
                            signature.Add("sealed");
                        }

                        break;
                    }
                }

                signature.Add(type.IsRecordType() ? "record" : "class");
            }
            else if (type.IsInterface)
            {
                signature.Add("interface");
            }
            else if (type.IsEnum)
            {
                signature.Add("enum");
            }
            else if (type.IsValueType)
            {
                signature.Add("struct");
            }
        }

        signature.Add(type.GetDisplayName());

        if (type.IsClass || type.IsInterface)
        {
            List<Type> baseTypeAndInterfaces = new();

            if (type.IsClass && type.BaseType != null && type.BaseType != typeof(object))
            {
                baseTypeAndInterfaces.Add(type.BaseType);
            }

            baseTypeAndInterfaces.AddRange(type.GetInterfaces().Where(x=> x.IsPublic));

            if (baseTypeAndInterfaces.Count > 0)
            {
                signature.Add($": {string.Join(", ", baseTypeAndInterfaces.Select(t => t.GetDisplayName()))}");
            }
        }

        return string.Join(' ', signature);
    }

    internal static string GetDisplayName(this Type type, bool simplifyName = false)
    {
        string name = simplifyName ? type.GetSimplifiedName() : type.Name;
        if (type.IsByRef)
        {
            name = name.Substring(0, name.Length - 1);
        }

        TypeInfo typeInfo = type.GetTypeInfo();
        Type[] genericParams = typeInfo.GenericTypeArguments.Length > 0 ? typeInfo.GenericTypeArguments : typeInfo.GenericTypeParameters;

        if (genericParams.Length > 0)
        {
            name = name[..name.IndexOf('`')];
            name += $"<{string.Join(", ", genericParams.Select(t => t.GetDisplayName(simplifyName)))}>";
        }

        return name;
    }

    internal static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
    {
        for (Type current = type; current != null; current = current.BaseType)
        {
            yield return current;
        }
    }

    internal static string GetMSDocsUrl(this Type type, string msdocsBaseUrl = "https://learn.microsoft.com/en-us/dotnet/api")
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }

        return $"{msdocsBaseUrl}/{type.GetDocsFileName(true)}";
    }

    internal static string GetInternalDocsUrl(this Type type, string currentNamespace,
        bool noExtension = false, bool noPrefix = false)
    {
        if (type == null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        
        var referenceTypeFolder = type.Assembly.GetRelativeFolderPath(type.Namespace);
        var currentTypeFolder = type.Assembly.GetRelativeFolderPath(currentNamespace);
        string ret = "";
        if (referenceTypeFolder != currentTypeFolder)
        {
            var y = currentTypeFolder?.Split("/").Where(x => !string.IsNullOrWhiteSpace(x)).ToArray().Length ?? 0;
            for (int i = 0; i < y; i++)
            {
                ret += "../";
            }

            if (!string.IsNullOrWhiteSpace(referenceTypeFolder))
            {
                if (referenceTypeFolder.ElementAt(0).Equals('/'))
                {
                    referenceTypeFolder = referenceTypeFolder.Remove(0, 1);
                }
                ret += referenceTypeFolder + "/";
            }
        }

        string url = $"{ret}{type.GetDocsFileName(false)}";

        if (!noExtension)
        {
            url += ".md";
        }

        if (!noPrefix)
        {
           if (url.ElementAt(0).Equals('/'))
           {
               url = Path.Join("." , url);
           }
           else
           {
               url = Path.Combine("./", url);
           }
           
        }

        if (url.Contains("//"))
        {
            url = url.Replace("//", "/");
        }

        return url;
    }

    internal static bool IsGenericParameter(this Type type)
    {
        if (type.IsGenericTypeParameter || type.IsGenericParameter)
        {
            return true;
        }

        if (type.IsArray)
        {
            var elementType = type.GetElementType();
            if (elementType != null)
            {
                return elementType.IsGenericTypeParameter || elementType.IsGenericParameter;
            }
        }

        return false;
    }
    
    internal static string GetDocsFileName(this Type type, bool fullNameSpace)
    {
        RequiredArgument.NotNull(type, nameof(type));
        string t = fullNameSpace ? type.FullName : type.Name;
        if (type.IsByRef)
        {
            t = t.Substring(0, t.Length - 1);
        }
        t = Regex.Replace(t, @"\[.*\]", string.Empty).Replace('+', '.');
        return t.ToLower().Replace('`', '-');
    }

    internal static MarkdownInlineElement GetDocsLink(
        this Type type,
        Assembly assembly,
        string currentNamespace,
        string text = null,
        bool noExtension = false,
        bool noPrefix = false)
    {
        if (string.IsNullOrEmpty(text))
        {
            text = type.GetDisplayName().FormatChevrons();
        }

        if (!type.IsGenericParameter())
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }

            if (type.Assembly == typeof(string).Assembly)
            {
                return new MarkdownLink(text, type.GetMSDocsUrl());
            }

            if (type.Assembly == assembly)
            {
                return new MarkdownLink(text, type.GetInternalDocsUrl(currentNamespace, noExtension, noPrefix));
            }
        }

        return new MarkdownText(text);
    }
}
