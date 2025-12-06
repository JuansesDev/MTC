using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using {{Name}}.Application.Common.Interfaces;
using {{Name}}.Domain.Entities;

namespace {{Name}}.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodoItemsController : ControllerBase
{
    private readonly IApplicationDbContext _context;

    public TodoItemsController(IApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<TodoItem>> Get()
    {
        return await _context.TodoItems.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<TodoItem>> Post(TodoItem item)
    {
        _context.TodoItems.Add(item);
        await _context.SaveChangesAsync(CancellationToken.None);
        return CreatedAtAction(nameof(Get), new { id = item.Id }, item);
    }
}
