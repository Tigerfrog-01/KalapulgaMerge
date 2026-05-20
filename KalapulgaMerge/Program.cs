using KalapulgaMerge.ApplicationServices.Services;
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