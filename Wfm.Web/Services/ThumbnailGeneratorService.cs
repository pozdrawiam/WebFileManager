using System.Diagnostics;
using Wfm.Domain.Consts;
using Wfm.Domain.Services;
using Wfm.Domain.Services.Settings;

namespace Wfm.Web.Services;

public class ThumbnailGeneratorService : BackgroundService
{
    private static readonly TimeSpan TimeLimit = TimeSpan.FromHours(1);

    private readonly ILogger<ThumbnailGeneratorService> _logger;
    private readonly IImageService _imageService;
    private readonly ISettingService _settingService;

    public ThumbnailGeneratorService(
        ILogger<ThumbnailGeneratorService> logger,
        ISettingService settingService,
        IImageService imageService)
    {
        _logger = logger;
        _settingService = settingService;
        _imageService = imageService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            var timer = Stopwatch.StartNew();

            foreach (LocationOptions location in _settingService.StorageOptions.Locations)
                GenerateMissingThumbnails(location, timer, stoppingToken);

        }, stoppingToken);
    }

    private void GenerateMissingThumbnails(LocationOptions location, Stopwatch timer, CancellationToken stoppingToken)
    {
        string[] imageFiles = Directory.GetFiles(location.Path, "*.*", SearchOption.AllDirectories)
            .Where(file =>!file.Contains(ThumbnailConsts.DirName) && ThumbnailConsts.Extensions.Any(ext => file.EndsWith("." + ext, StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        if (imageFiles.Length == 0)
            return;

        _logger.LogInformation("Generate missing {} thumbnails for {}", imageFiles.Length, location.Name);

        int generatedThumbnails = 0;

        foreach (string imageFile in imageFiles)
        {
            if (timer.Elapsed > TimeLimit)
            {
                _logger.LogInformation("Generate missing thumbnails stopped by time limit, created {} thumbnails", generatedThumbnails);
                return;
            }

            if (stoppingToken.IsCancellationRequested)
                return;

            if (string.IsNullOrEmpty(imageFile))
                continue;

            string thumbnailDirectory = Path.Combine(Path.GetDirectoryName(imageFile)!, ThumbnailConsts.DirName);
            string thumbnailFileName = Path.GetFileName(imageFile);
            string thumbnailPath = Path.Combine(thumbnailDirectory, thumbnailFileName);

            if (!File.Exists(thumbnailPath))
            {
                if (!Directory.Exists(thumbnailDirectory))
                    Directory.CreateDirectory(thumbnailDirectory);

                _logger.LogTrace("Generate missing thumbnail for '{}'", imageFile);
                GenerateThumbnail(imageFile, thumbnailPath);
                generatedThumbnails++;
            }
        }

        _logger.LogInformation("Generated {} missing thumbnails", generatedThumbnails);
    }

    private void GenerateThumbnail(string sourcePath, string destinationPath)
    {
        _imageService.CreateThumbnail(sourcePath, destinationPath, ThumbnailConsts.MaxWidth, ThumbnailConsts.MaxHeight);
    }
}
