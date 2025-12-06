using Microsoft.EntityFrameworkCore;
using {{Name}}.Features.Todos;

namespace {{Name}}.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
