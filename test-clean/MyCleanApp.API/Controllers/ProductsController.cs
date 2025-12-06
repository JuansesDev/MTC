using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyCleanApp.Application.Features.Products.Commands.CreateProduct;
using MyCleanApp.Application.Features.Products.Queries.GetProducts;
using MyCleanApp.Domain.Entities;

namespace MyCleanApp.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IEnumerable<Product>> Get()
    {
        return await _mediator.Send(new GetProductsQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateProductCommand command)
    {
        return await _mediator.Send(command);
    }
}
