using MediatR;
using Microsoft.EntityFrameworkCore;
using MyCleanApp.Application.Common.Interfaces;
using MyCleanApp.Domain.Entities;

namespace MyCleanApp.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<Product>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<Product>>
{
    private readonly IApplicationDbContext _context;

    public GetProductsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Set<Product>().ToListAsync(cancellationToken);
    }
}
