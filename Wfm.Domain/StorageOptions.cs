namespace Wfm.Domain;

public class StorageOptions
{
    public const string Storage = "Storage";
    
    public LocationOption[] Locations { get; set; } = Array.Empty<LocationOption>();
}

public class LocationOption
{
    public string Name { get; set; }
    public string Path { get; set; }
}
