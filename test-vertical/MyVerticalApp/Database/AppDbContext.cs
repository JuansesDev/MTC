using Microsoft.EntityFrameworkCore;
using MyVerticalApp.Features.Todos;

namespace MyVerticalApp.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
