using Microsoft.Extensions.Options;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;

namespace Wfm.Web.Services;

public class SettingService : ISettingService
{
    private readonly StorageOptions _storageOptions;
    
    public SettingService(IOptions<StorageOptions> storageOptions)
    {
        _storageOptions = storageOptions.Value;
    }
    
    public StorageOptions StorageOptions => _storageOptions;
}
