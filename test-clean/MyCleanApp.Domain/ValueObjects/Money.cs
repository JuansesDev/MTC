namespace MyCleanApp.Domain.ValueObjects;

public record Money
{
    
        public decimal Amount { get; init; }
        
        public string Currency { get; init; }
        
}
