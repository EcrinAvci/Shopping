using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;


namespace StoreApp.Infrastructere.Extensions
{
    public static class ApplicationExtension
    {
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            RepositoryContext context =app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<RepositoryContext>();

            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static void ConfigureLocalization(this WebApplication app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("tr-TR")
                    .AddSupportedUICultures("tr-TR")
                    .SetDefaultCulture("tr-TR");
            });
        }
   
        public static async Task ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser = "Admin";
            const string adminPassword = "Admin+123456";
            const string adminEmail = "eco03gfb@gmail.com";
            const string adminPhone = "5362610740";

            using (var scope = app.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Rolleri kontrol et ve yoksa ekle
                string[] roles = new[] { "Admin", "User", "Editor" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }

                // Admin kullanıcıyı kontrol et ve yoksa ekle
                var user = await userManager.FindByNameAsync(adminUser);
                if (user == null)
                {
                    user = new IdentityUser(adminUser)
                    {
                        Email = adminEmail,
                        PhoneNumber = adminPhone,
                        UserName = adminUser,
                    };
                    var result = await userManager.CreateAsync(user, adminPassword);
                    if (!result.Succeeded)
                        throw new Exception("Admin kullanıcı oluşturulamadı: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                // Admin rolü atanmış mı kontrol et, yoksa ata
                if (!await userManager.IsInRoleAsync(user, "Admin"))
                {
                    var roleResult = await userManager.AddToRoleAsync(user, "Admin");
                    if (!roleResult.Succeeded)
                        throw new Exception("Admin kullanıcıya rol atanamadı: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}