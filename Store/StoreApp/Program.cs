using StoreApp.Infrastructere.Extensions;

var builder = WebApplication.CreateBuilder(args);

// HTTP üzerinden çalışması için ayarları değiştiriyorum
builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureSession();
builder.Services.ConfigureRepositoryRegistration();
builder.Services.ConfigureServiceRegistration();
builder.Services.ConfigureRouting();
builder.Services.ConfigureIdentity();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => 
{
    endpoints.MapAreaControllerRoute(
        name:"Admin",
        areaName:"Admin",
        pattern:"Admin/{controller=Dashboard}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
    endpoints.MapControllers();
});

app.ConfigureAndCheckMigration();
app.ConfigureLocalization();
await app.ConfigureDefaultAdminUser();
app.Run();
