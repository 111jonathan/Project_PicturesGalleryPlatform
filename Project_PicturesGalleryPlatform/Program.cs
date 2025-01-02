using Project_PicturesGalleryPlatform.Repositories.ImageRepository;
using Project_PicturesGalleryPlatform.Repositories.MyFavoritesRepository;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService.PythonImageAnalysisExecutor;
using Project_PicturesGalleryPlatform.Services.ImageAnalysisService;
using Project_PicturesGalleryPlatform.Services.ImageService;
using Project_PicturesGalleryPlatform.Services.MyFavoritesService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IMyFavoritesRepository, MyFavoritesRepository>();
builder.Services.AddScoped<IMyFavoritesService, MyFavoritesService>();

builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IImageService, ImageService>();

builder.Services.AddScoped<IImageAnalysisService, ImageAnalysisService>();
builder.Services.AddScoped<IPythonImageAnalysisExecutor, PythonImageAnalysisExecutor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
