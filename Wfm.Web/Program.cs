using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;
using Wfm.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<IFileSystemService, FileSystemService>();

builder.Services.AddTransient<GetFilesHandler>();

var app = builder.Build();

app.MapGet("/", (GetFilesHandler handl) => 
{
    return handl.Handle(new GetFilesQuery(0, "New folder"));
});

app.Run();
