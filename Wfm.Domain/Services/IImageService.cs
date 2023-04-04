namespace Wfm.Domain.Services;

public interface IImageService
{
    void CreateThumbnail(string sourcePath, string destinationPath, int width, int height);
}
