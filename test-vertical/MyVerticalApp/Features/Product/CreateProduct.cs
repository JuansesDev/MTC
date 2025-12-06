using MediatR;
using MyVerticalApp.Database;

namespace MyVerticalApp.Features.Products;

public record CreateProductCommand(
    
        string Name,
        
        decimal Price,
        
        bool InStock
        
) : IRequest<int>;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly AppDbContext _db;

    public CreateProductHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            
                        Name = request.Name,
                        
                        Price = request.Price,
                        
                        InStock = request.InStock,
                        
        };

        _db.Set<Product>().Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
