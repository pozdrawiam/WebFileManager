namespace Wfm.Domain.Settings;

public record LocationOptions
{
    public string? Name { get; init; }
    public string Path { get; init; } = "";
}
