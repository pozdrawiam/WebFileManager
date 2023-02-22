using Wfm.Domain.Features.FileManager.FileList;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;
using Wfm.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<IFileSystemService, FileSystemService>();

builder.Services.AddTransient<FileListQueryHandler>();

var app = builder.Build();

app.MapGet("/", (FileListQueryHandler handl) => 
{
    return handl.Handle(new FileListQuery(0, "New folder"));
});

app.Run();
