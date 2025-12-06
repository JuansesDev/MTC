using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVerticalApp.Database;

namespace MyVerticalApp.Features.Todos;

public record GetTodosQuery : IRequest<List<Todo>>;

public class GetTodosHandler : IRequestHandler<GetTodosQuery, List<Todo>>
{
    private readonly AppDbContext _db;

    public GetTodosHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Todo>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        return await _db.Todos.ToListAsync(cancellationToken);
    }
}
