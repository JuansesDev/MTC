namespace {{Namespace}};

public class {{Name}}
{
    {{ for prop in Properties }}
    public {{ prop.Type }} {{ prop.Name }} { get; set; }
    {{ end }}
}
