namespace MapSolution.Domain.Models.Get;

public record ShopDetailDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
    
    public decimal Profit { get; init; }
    public decimal Expenses { get; init; }
}