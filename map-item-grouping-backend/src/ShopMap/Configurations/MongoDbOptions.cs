namespace MapSolution.Configurations;

public record MongoOptions
{
    public static readonly string Section = "MONGODB";

    public string USERNAME { get; init; } = string.Empty;
    public string PASSWORD { get; init; } = string.Empty;
    public string HOST { get; init; } = string.Empty;
    public string PORT { get; init; } = string.Empty;
    public string NAME { get; init; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrEmpty(HOST) ||
            string.IsNullOrEmpty(PORT) ||
            string.IsNullOrEmpty(NAME))
        {
            throw new ArgumentNullException(Section);
        }
    }
}