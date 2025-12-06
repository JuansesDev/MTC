using {{ProjectName}}.Domain.Common;

namespace {{ProjectName}}.Domain.Entities;

public class {{Name}} : BaseEntity
{
    {{ for prop in Properties }}
    public {{ prop.Type }} {{ prop.Name }} { get; set; }
    {{ end }}
}
