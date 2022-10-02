using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Repository;
using IdentityServer.AuthServer.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Reflection;

namespace IdentityServer.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomUserRepository, CustomUserRepository>();

            services.AddDbContext<CustomDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("LocalDb"));
            });

            var assmeblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = c =>
                    {
                        c.UseSqlServer(Configuration.GetConnectionString("LocalDb"), sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(assmeblyName);
                        });
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = c =>
                    {
                        c.UseSqlServer(Configuration.GetConnectionString("LocalDb"), sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(assmeblyName);
                        });
                    };
                })
                //.AddInMemoryApiResources(Config.GetApiResources())
                //.AddInMemoryApiScopes(Config.GetApiScopes())
                //.AddInMemoryClients(Config.GetClients())
                //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetUsers().ToList())
                .AddDeveloperSigningCredential()
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}