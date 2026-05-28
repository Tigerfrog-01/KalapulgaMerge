using KalapulgaMerge.ApplicationServices.Services;
using KalapulgaMerge.Core.Domain;
using KalapulgaMerge.Core.ServiceInterface;
using KalapulgaMerge.Data;
using Microsoft.EntityFrameworkCore;

namespace KalapulgaMerge
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<KalapulkDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("KalapulgaMerge.Data")
                ));

            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IShopService, ShopServices>();

            builder.Services.AddScoped<IFilesServices, FilesServices>();
            builder.Services.AddScoped<ICaseService, CaseService>();

            builder.Services.AddSession();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<KalapulkDbContext>();
                    db.Database.Migrate();

                    var adminByNewEmail = db.UserAccounts.FirstOrDefault(x => x.Email == "admin@admin");
                    var adminByOldEmail = db.UserAccounts.FirstOrDefault(x => x.Email == "admin");
                    var adminByName = db.UserAccounts.FirstOrDefault(x => x.Name == "admin");
                    var admin = adminByNewEmail ?? adminByOldEmail ?? adminByName;

                    if (admin == null)
                    {
                        db.UserAccounts.Add(new UserAccount
                        {
                            Name = "admin",
                            Email = "admin@admin",
                            Password = "admin"
                        });
                    }
                    else
                    {
                        admin.Name = "admin";
                        admin.Email = "admin@admin";
                        admin.Password = "admin";
                    }

                    db.SaveChanges();
                }
                catch
                {
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
