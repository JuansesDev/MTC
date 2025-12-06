using Microsoft.EntityFrameworkCore;
using {{Name}}.Domain.Entities;

namespace {{Name}}.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
