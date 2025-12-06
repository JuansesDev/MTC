using Microsoft.EntityFrameworkCore;
using MyCleanApp.Application.Common.Interfaces;
using MyCleanApp.Domain.Entities;

namespace MyCleanApp.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems => Set<TodoItem>();
}
