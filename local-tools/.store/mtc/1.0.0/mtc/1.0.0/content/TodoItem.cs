using {{Name}}.Domain.Common;

namespace {{Name}}.Domain.Entities;

public class TodoItem : BaseEntity
{
    public string? Title { get; set; }
    public string? Note { get; set; }
    public bool Done { get; set; }
}
