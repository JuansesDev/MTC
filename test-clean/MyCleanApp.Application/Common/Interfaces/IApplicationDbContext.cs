using Microsoft.EntityFrameworkCore;
using MyCleanApp.Domain.Entities;

namespace MyCleanApp.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoItem> TodoItems { get; }
    DbSet<T> Set<T>() where T : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
