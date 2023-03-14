namespace Wfm.Domain.Services.Settings;

public interface ISettingService
{
    StorageOptions StorageOptions { get; }

    LocationOptions? GetLocationByIndex(int index);
}
