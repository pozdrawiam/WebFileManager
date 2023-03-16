namespace Wfm.Domain.Features.FileManager.GetThumbnail;

public class GetThumbnailHandler
{
    public GetThumbnailResult Handle(GetThumbnailQuery query)
    {
        var thumbnailPath = Path.Join(Path.GetDirectoryName(query.ImagePath), ".thumbnails", Path.GetFileName(query.ImagePath));

        throw new NotImplementedException();
    }
}
