using Microsoft.AspNetCore.Mvc;
using MediatR;
using {{ProjectName}}.Application.Features.{{Name}}s.Commands.Create{{Name}};
using {{ProjectName}}.Application.Features.{{Name}}s.Queries.Get{{Name}}s;
using {{ProjectName}}.Domain.Entities;

namespace {{ProjectName}}.API.Controllers;

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
    public async Task<IEnumerable<{{Name}}>> Get()
    {
        return await _mediator.Send(new Get{{Name}}sQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(Create{{Name}}Command command)
    {
        return await _mediator.Send(command);
    }
}
