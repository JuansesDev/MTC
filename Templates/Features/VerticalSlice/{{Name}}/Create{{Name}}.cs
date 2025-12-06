using MediatR;
using {{ProjectName}}.Database;

namespace {{ProjectName}}.Features.{{Name}}s;

public record Create{{Name}}Command(
    {{ for prop in Properties }}
    {{ prop.Type }} {{ prop.Name }}{{ if !for.last }},{{ end }}
    {{ end }}
) : IRequest<int>;

public class Create{{Name}}Handler : IRequestHandler<Create{{Name}}Command, int>
{
    private readonly AppDbContext _db;

    public Create{{Name}}Handler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(Create{{Name}}Command request, CancellationToken cancellationToken)
    {
        var entity = new {{Name}}
        {
            {{ for prop in Properties }}
            {{ prop.Name }} = request.{{ prop.Name }},
            {{ end }}
        };

        _db.Set<{{Name}}>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
