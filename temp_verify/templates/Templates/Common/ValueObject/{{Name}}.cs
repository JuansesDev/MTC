namespace {{Namespace}};

public record {{Name}}
{
    {{ for prop in Properties }}
    public {{ prop.Type }} {{ prop.Name }} { get; init; }
    {{ end }}
}
