namespace MapSolution.Domain.Models.Create;

public record CreateShopInputModel
{
    public string Name { get; init; }
    
    public string City { get; init; }
    public string PostalCode { get; init; }
    public string Street { get; init; }
    public string Number { get; init; }
    public decimal Latitude { get; init; }
    public decimal Longitude { get; init; }
}