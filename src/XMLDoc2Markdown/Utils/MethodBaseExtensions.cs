using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Markdown;

namespace XMLDoc2Markdown.Utils;

internal static class MethodBaseExtensions
{
    internal static Visibility GetVisibility(this MethodBase methodBase)
    {
        if (methodBase.IsPublic)
        {
            return Visibility.Public;
        }
        else if (methodBase.IsAssembly)
        {
            return Visibility.Internal;
        }
        else if (methodBase.IsFamily)
        {
            return Visibility.Protected;
        }
        else if (methodBase.IsFamilyOrAssembly)
        {
            return Visibility.ProtectedInternal;
        }
        else if (methodBase.IsPrivate)
        {
            return Visibility.Private;
        }
        else
        {
            return Visibility.None;
        }
    }

    internal static string GetSignature(this MethodBase methodBase, bool full = false)
    {
        List<string> signature = new();

        if (full)
        {
            if (methodBase.DeclaringType.IsClass || methodBase.DeclaringType.IsValueType)
            {
                signature.Add(methodBase.GetVisibility().Print());

                if (methodBase.IsStatic)
                {
                    signature.Add("static");
                }

                if (methodBase.IsAbstract)
                {
                    signature.Add("abstract");
                }
            }

            if (methodBase is MethodInfo methodInfo)
            {
                signature.Add(methodInfo.ReturnType.GetDisplayName(simplifyName: true));
            }
        }

        string displayName = methodBase.MemberType == MemberTypes.Constructor ? methodBase.DeclaringType.Name : methodBase.Name;
        int genericCharIndex = displayName.IndexOf('`');
        if (genericCharIndex > -1)
        {
            displayName = displayName[..genericCharIndex];
        }
        if (methodBase is MethodInfo methodInfo1)
        {
            Type[] genericArguments = methodInfo1.GetGenericArguments();
            if (genericArguments.Length > 0)
            {
                displayName += $"<{string.Join(", ", genericArguments.Select(a => a.Name))}>";
            }
        }
        ParameterInfo[] @params = methodBase.GetParameters();
        IEnumerable<string> paramsNames = @params
            .Select(p => $"{(p.ParameterType.IsByRef ? "out ": "")}{p.ParameterType.GetDisplayName(simplifyName: full)}{(full ? $" {p.Name}" : null)}");
        displayName += $"({string.Join(", ", paramsNames)})";
        signature.Add(displayName);

        return string.Join(' ', signature);
    }

    internal static string GetMSDocsUrl(this MethodBase methodInfo, string msdocsBaseUrl = "https://docs.microsoft.com/en-us/dotnet/api")
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

        if (type.Assembly != typeof(string).Assembly)
        {
            throw new InvalidOperationException($"{type.FullName} is not a mscorlib type.");
        }

        return $"{msdocsBaseUrl}/{type.GetDocsFileName(true)}.{methodInfo.Name.ToLower().Replace('`', '-')}";
    }

    internal static string GetInternalDocsUrl(this MethodBase methodInfo, string currentNamespace, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));

        Type type = methodInfo.DeclaringType ?? throw new Exception($"Method {methodInfo.Name} has no declaring type.");

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


        string anchor = methodInfo.GetSignature().ToAnchorLink();

        return $"{url}#{anchor}";
    }

    internal static MarkdownInlineElement GetDocsLink(this MethodBase methodInfo, Assembly assembly, string currentNamespace, string text = null, bool noExtension = false, bool noPrefix = false)
    {
        RequiredArgument.NotNull(methodInfo, nameof(methodInfo));
        RequiredArgument.NotNull(assembly, nameof(assembly));
        RequiredArgument.NotNull(currentNamespace, nameof(currentNamespace));

        Type type = methodInfo.DeclaringType;

        if (type is not null)
        {
            if (string.IsNullOrEmpty(text))
            {
                text = $"{type.GetDisplayName().FormatChevrons()}.{methodInfo.GetSignature().FormatChevrons()}";
            }

            if (type.Assembly == typeof(string).Assembly)
            {
                return new MarkdownLink(text, methodInfo.GetMSDocsUrl());
            }

            if (type.Assembly == assembly)
            {
                return new MarkdownLink(text, methodInfo.GetInternalDocsUrl(currentNamespace, noExtension, noPrefix));
            }

            return new MarkdownText(text);
        }

        return new MarkdownText(text ?? methodInfo.Name);
    }
}
