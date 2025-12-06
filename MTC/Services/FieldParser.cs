using MTC.Models;

namespace MTC.Services;

public class FieldParser : IFieldParser
{
    public List<Property> Parse(string input)
    {
        var properties = new List<Property>();
        if (string.IsNullOrWhiteSpace(input))
        {
            return properties;
        }

        var fields = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        foreach (var field in fields)
        {
            var parts = field.Split(':');
            if (parts.Length == 2)
            {
                properties.Add(new Property
                {
                    Name = parts[0],
                    Type = MapType(parts[1])
                });
            }
        }

        return properties;
    }

    private string MapType(string type)
    {
        return type.ToLower() switch
        {
            "string" => "string",
            "int" => "int",
            "bool" => "bool",
            "decimal" => "decimal",
            "datetime" => "DateTime",
            "guid" => "Guid",
            _ => type // Fallback to provided type
        };
    }
}
