using Microsoft.Extensions.Options;
using Wfm.Domain.Services.Settings;

namespace Wfm.Web.Services;

public class SettingService : ISettingService
{
    public SettingService(IOptions<StorageOptions> storageOptions)
    {
        StorageOptions = storageOptions.Value;
    }

    public StorageOptions StorageOptions { get; }

    public LocationOptions? GetLocationByIndex(int index)
    {
        return StorageOptions.Locations?.ElementAt(index);
    }
}
