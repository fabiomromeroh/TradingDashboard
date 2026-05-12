using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Nodes;
using System.Xml.Linq;

namespace TradingDashboard.API.Swagger;

public class EnumSchemaFilter : ISchemaFilter
{
    private readonly XDocument? _xmlDoc;

    public EnumSchemaFilter()
    {
        var allMembers = Directory
            .GetFiles(AppContext.BaseDirectory, "TradingDashboard.*.xml")
            .Where(File.Exists)
            .Select(XDocument.Load)
            .SelectMany(doc => doc.Descendants("member"))
            .ToList();

        if (allMembers.Count > 0)
            _xmlDoc = new XDocument(new XElement("doc", new XElement("members", allMembers)));
    }

    public void Apply(IOpenApiSchema schema, SchemaFilterContext context)
    {
        if (!context.Type.IsEnum)
            return;

        if (schema is not OpenApiSchema concreteSchema)
            return;

        // Switch schema type from integer to string
        concreteSchema.Type = JsonSchemaType.String;
        concreteSchema.Format = null;
        concreteSchema.Enum = [];

        var enumDescriptions = new List<string>();

        foreach (var value in Enum.GetValues(context.Type))
        {
            var name = value.ToString()!;
            concreteSchema.Enum.Add(JsonValue.Create(name)!);

            // Look up XML <summary> for this enum member
            var memberName = $"F:{context.Type.FullName}.{name}";
            var xmlSummary = _xmlDoc?
                .Descendants("member")
                .FirstOrDefault(m => m.Attribute("name")?.Value == memberName)
                ?.Element("summary")?.Value.Trim();

            enumDescriptions.Add(string.IsNullOrWhiteSpace(xmlSummary)
                ? $"<b>{name}</b>"
                : $"<b>{name}</b>: {xmlSummary}");
        }

        // Append per-value descriptions to the schema description
        var enumDoc = string.Join("<br/>", enumDescriptions);
        concreteSchema.Description = string.IsNullOrWhiteSpace(concreteSchema.Description)
            ? enumDoc
            : $"{concreteSchema.Description}<br/><br/>{enumDoc}";
    }
}
