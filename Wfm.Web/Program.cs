using Serilog;
using Wfm.Domain.Features.FileManager.DownloadFile;
using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Features.FileManager.GetThumbnail;
using Wfm.Domain.Services;
using Wfm.Domain.Services.FileSystem;
using Wfm.Domain.Services.Settings;
using Wfm.Web.Core;
using Wfm.Web.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Services.AddLogging(x => x.ClearProviders().AddSerilog());

builder.Services.AddControllersWithViews();

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

builder.Services.AddTransient(typeof(Lazy<>), typeof(Lazier<>));

builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<IFileSystemService, FileSystemService>();
builder.Services.AddTransient<IImageService, ImageService>();

builder.Services.AddTransient<DownloadFileHandler>();
builder.Services.AddTransient<GetFilesHandler>();
builder.Services.AddTransient<GetThumbnailHandler>();

builder.Services.AddHostedService<ThumbnailGeneratorService>();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Logger.LogInformation("Starting app at {Date}", DateTime.Now);

app.Run();

Log.CloseAndFlush();
