namespace MapSolution.Domain.Models.Get;

public record ShopListDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Profit { get; init; }
    public decimal Expenses { get; init; }
}