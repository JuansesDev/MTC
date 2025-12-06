using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MyVerticalApp.Features.Todos;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<Todo>>> Get()
    {
        return await _mediator.Send(new GetTodosQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoCommand command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}
