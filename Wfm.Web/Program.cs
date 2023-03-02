using Wfm.Domain.Features.FileManager.GetFiles;
using Wfm.Domain.Services;
using Wfm.Domain.Settings;
using Wfm.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.Storage)
);

builder.Services.AddTransient<ISettingService, SettingService>();
builder.Services.AddTransient<IFileSystemService, FileSystemService>();

builder.Services.AddTransient<GetFilesHandler>();

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

app.Run();
