using MediatR;
using {{ProjectName}}.Application.Common.Interfaces;
using {{ProjectName}}.Domain.Entities;

namespace {{ProjectName}}.Application.Features.{{Name}}s.Commands.Create{{Name}};

public record Create{{Name}}Command : IRequest<int>
{
    {{ for prop in Properties }}
    public {{ prop.Type }} {{ prop.Name }} { get; init; }
    {{ end }}
}

public class Create{{Name}}CommandHandler : IRequestHandler<Create{{Name}}Command, int>
{
    private readonly IApplicationDbContext _context;

    public Create{{Name}}CommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(Create{{Name}}Command request, CancellationToken cancellationToken)
    {
        var entity = new {{Name}}
        {
            {{ for prop in Properties }}
            {{ prop.Name }} = request.{{ prop.Name }},
            {{ end }}
        };

        _context.Set<{{Name}}>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
