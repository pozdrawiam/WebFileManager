namespace Wfm.Web.Services;

public class ImageService
{
    public void CreateThumbnail(string sourcePath, string destinationPath, int width, int height)
    {
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
