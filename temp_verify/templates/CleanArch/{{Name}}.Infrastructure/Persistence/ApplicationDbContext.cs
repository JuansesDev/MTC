using Microsoft.EntityFrameworkCore;
using {{Name}}.Application.Common.Interfaces;
using {{Name}}.Domain.Entities;

namespace {{Name}}.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}
