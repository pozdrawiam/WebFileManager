namespace Wfm.Domain.Services.Settings;

public record StorageOptions
{
    public const string Storage = "Storage";

    public LocationOptions[] Locations { get; set; } = Array.Empty<LocationOptions>();
}
