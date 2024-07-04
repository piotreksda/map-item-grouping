namespace MapSolution.Configurations;

public record MongoOptions
{
    public static readonly string Section = "MongoDB";

    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public string Host { get; init; } = string.Empty;
    public string Port { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrEmpty(Host) ||
            string.IsNullOrEmpty(Port) ||
            string.IsNullOrEmpty(Name))
        {
            throw new ArgumentNullException(Section);
        }
    }
}