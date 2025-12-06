namespace {{ProjectName}}.Features.{{Name}}s;

public class {{Name}}
{
    public int Id { get; set; }
    {{ for prop in Properties }}
    public {{ prop.Type }} {{ prop.Name }} { get; set; }
    {{ end }}
}
