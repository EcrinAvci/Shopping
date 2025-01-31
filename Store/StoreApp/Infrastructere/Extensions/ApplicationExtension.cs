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
   
        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser="Admin";
            const string adminPassword="Admin+123456";

            UserManager<IdentityUser> userManager = app
            .ApplicationServices
            .CreateScope()
            .ServiceProvider
            .GetRequiredService<UserManager<IdentityUser>>();

            RoleManager<IdentityRole> roleManager=app
            .ApplicationServices
            .CreateAsyncScope()
            .ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

            IdentityUser user = await userManager.FindByNameAsync(adminUser);

            if(user is null)
            {
                user=new IdentityUser(adminUser)
                {
                    Email= "eco03gfb@gmail.com",
                    PhoneNumber="5362610740",
                    UserName=adminUser,
                };

                var result = await userManager.CreateAsync(user,adminPassword);
                if(!result.Succeeded)
                    throw new Exception("Admin user not creaded");

                var roleResult= await userManager.AddToRolesAsync(user,
                roleManager
                    .Roles
                    .Select(r => r.Name)
                    .ToList()
                );

                if(!roleResult.Succeeded)
                    throw new Exception("Mistake");    
            }
        }
    }
}