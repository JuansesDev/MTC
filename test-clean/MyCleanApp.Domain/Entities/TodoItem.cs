using MyCleanApp.Domain.Common;

namespace MyCleanApp.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string? Title { get; set; }
    public string? Note { get; set; }
    public bool Done { get; set; }
}
