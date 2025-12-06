using MediatR;
using Microsoft.EntityFrameworkCore;
using MyVerticalApp.Database;

namespace MyVerticalApp.Features.Products;

public record GetProductsQuery : IRequest<List<Product>>;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<Product>>
{
    private readonly AppDbContext _db;

    public GetProductsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Set<Product>().ToListAsync(cancellationToken);
    }
}
