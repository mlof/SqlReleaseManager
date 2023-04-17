using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Dac.Model;
using SqlReleaseManager.Core.Constants;
using SqlReleaseManager.Core.Persistence;
using SqlReleaseManager.Identity.Persistence;

namespace SqlReleaseManager.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var applicationDbConnectionString = builder.Configuration.GetConnectionString("ApplicationDb") ??
                                                throw new InvalidOperationException(
                                                    "Connection string 'DefaultConnection' not found.");
            var identityDbConnectionString = builder.Configuration.GetConnectionString("IdentityDb") ??
                                             throw new InvalidOperationException(
                                                 "Connection string 'IdentityDb' not found.");

            builder.Services.AddDbContext<IdentityContext>(options =>
                ConfigureDbContext<IdentityContext>(options, identityDbConnectionString)
            );
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                ConfigureDbContext<ApplicationDbContext>(options, applicationDbConnectionString)
            );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(Program)));


            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<IdentityContext>();
            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Database));


            builder.Services.AddControllersWithViews();

            var app = builder.Build();


            if (app.Services.GetRequiredService<IOptions<DatabaseOptions>>().Value.RunMigrationsOnStartup)
            {
                using var serviceScope = app.Services.CreateScope();
                var services = serviceScope.ServiceProvider;
                var identityContext = services.GetRequiredService<IdentityContext>();
                var applicationDbContext = services.GetRequiredService<ApplicationDbContext>();
                identityContext.Database.Migrate();
                applicationDbContext.Database.Migrate();
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.Run();
        }

        private static DbContextOptionsBuilder ConfigureDbContext<T>(DbContextOptionsBuilder options,
            string connectionString)
        {
            return options.UseSqlServer(connectionString,
                optionsBuilder => optionsBuilder.MigrationsAssembly(typeof(T).Assembly.GetName().Name));
        }
    }
}