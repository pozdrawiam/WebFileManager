using Microsoft.Extensions.Options;
using Wfm.Domain.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

var app = builder.Build();

app.MapGet("/", (IOptions<StorageOptions> opts) => 
{
    return opts.Value;
});

app.Run();
