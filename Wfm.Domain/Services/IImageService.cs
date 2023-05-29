namespace Wfm.Domain.Services;

public interface IImageService
{
    bool CreateThumbnail(string sourcePath, string destinationPath, int width, int height);
}
