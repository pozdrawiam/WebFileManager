using Wfm.Domain.Services;

namespace Wfm.Web.Services;

public class ImageService : IImageService
{
    private static readonly object CreateThumbnailLock = new();

    public void CreateThumbnail(string sourcePath, string destinationPath, int width, int height)
    {
        lock (CreateThumbnailLock)
        {
            if (File.Exists(destinationPath))
                return;

            using var image = Image.Load(sourcePath);

            image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max
                }));

            image.Save(destinationPath);
        }
    }
}
