using MediatR;
using MyCleanApp.Application.Common.Interfaces;
using MyCleanApp.Domain.Entities;

namespace MyCleanApp.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand : IRequest<int>
{
    
        public string Name { get; init; }
        
        public decimal Price { get; init; }
        
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var entity = new Product
        {
            
                        Name = request.Name,
                        
                        Price = request.Price,
                        
        };

        _context.Set<Product>().Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
