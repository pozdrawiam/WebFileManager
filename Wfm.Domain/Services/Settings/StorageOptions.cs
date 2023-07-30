namespace Wfm.Domain.Services.Settings;

public record StorageOptions
{
    public const string Storage = "Storage";

    public int ListPageSize { get; set; } = 100;

    public LocationOptions[] Locations { get; set; } = Array.Empty<LocationOptions>();
}
