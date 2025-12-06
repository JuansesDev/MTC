using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace {{ProjectName}}.Features.{{Name}}s;

[ApiController]
[Route("api/[controller]")]
public class {{Name}}sController : ControllerBase
{
    private readonly IMediator _mediator;

    public {{Name}}sController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<{{Name}}>>> Get()
    {
        return await _mediator.Send(new Get{{Name}}sQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(Create{{Name}}Command command)
    {
        var id = await _mediator.Send(command);
        return Ok(id);
    }
}
