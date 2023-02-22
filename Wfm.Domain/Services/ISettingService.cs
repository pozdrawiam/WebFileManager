using Wfm.Domain.Settings;

namespace Wfm.Domain.Services;

public interface ISettingService
{
    StorageOptions StorageOptions { get; }
}
