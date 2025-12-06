using MediatR;
using MyVerticalApp.Database;

namespace MyVerticalApp.Features.Todos;

public record CreateTodoCommand(string Title) : IRequest<int>;

public class CreateTodoHandler : IRequestHandler<CreateTodoCommand, int>
{
    private readonly AppDbContext _db;

    public CreateTodoHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo { Title = request.Title };
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync(cancellationToken);
        return todo.Id;
    }
}
