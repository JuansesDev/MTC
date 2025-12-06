using MediatR;
using Microsoft.EntityFrameworkCore;
using {{ProjectName}}.Database;

namespace {{ProjectName}}.Features.{{Name}}s;

public record Get{{Name}}sQuery : IRequest<List<{{Name}}>>;

public class Get{{Name}}sHandler : IRequestHandler<Get{{Name}}sQuery, List<{{Name}}>>
{
    private readonly AppDbContext _db;

    public Get{{Name}}sHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<{{Name}}>> Handle(Get{{Name}}sQuery request, CancellationToken cancellationToken)
    {
        return await _db.Set<{{Name}}>().ToListAsync(cancellationToken);
    }
}
