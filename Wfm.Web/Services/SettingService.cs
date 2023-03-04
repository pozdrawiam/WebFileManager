using Microsoft.Extensions.Options;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;

namespace Wfm.Web.Services;

public class SettingService : ISettingService
{
    public SettingService(IOptions<StorageOptions> storageOptions)
    {
        StorageOptions = storageOptions.Value;
    }

    public StorageOptions StorageOptions { get; }
}
