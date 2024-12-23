using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XMLDoc2Markdown.Docusaurus;

internal static class DocusaurusSerializer
{
    
    public static void Serialize(string folderPath, Category category)
    {
        var filePath = Path.Combine(folderPath, "_category_.json");

        using var sw = new StreamWriter(filePath);
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            WriteIndented = true
        };
        JsonSerializer.Serialize(sw.BaseStream, category,serializeOptions);
    }
}
