using Microsoft.Extensions.Options;
using Wfm.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

var app = builder.Build();

app.MapGet("/", (IOptions<StorageOptions> opts) => 
{
    return System.IO.Directory.Exists(opts.Value.Locations[0].Path);
});

app.Run();
