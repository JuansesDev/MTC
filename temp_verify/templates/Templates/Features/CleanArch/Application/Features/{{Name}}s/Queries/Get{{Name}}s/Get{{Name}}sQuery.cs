using MediatR;
using Microsoft.EntityFrameworkCore;
using {{ProjectName}}.Application.Common.Interfaces;
using {{ProjectName}}.Domain.Entities;

namespace {{ProjectName}}.Application.Features.{{Name}}s.Queries.Get{{Name}}s;

public record Get{{Name}}sQuery : IRequest<List<{{Name}}>>;

public class Get{{Name}}sQueryHandler : IRequestHandler<Get{{Name}}sQuery, List<{{Name}}>>
{
    private readonly IApplicationDbContext _context;

    public Get{{Name}}sQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<{{Name}}>> Handle(Get{{Name}}sQuery request, CancellationToken cancellationToken)
    {
        return await _context.Set<{{Name}}>().ToListAsync(cancellationToken);
    }
}
