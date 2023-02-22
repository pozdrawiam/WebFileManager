namespace Wfm.Domain.Settings;

public record StorageOptions
{
    public const string Storage = "Storage";
    
    public LocationOptions[] Locations { get; set; } = Array.Empty<LocationOptions>();
}
