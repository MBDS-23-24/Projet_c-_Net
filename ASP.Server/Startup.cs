using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ASP.Server.Database;


namespace ASP.Server
{

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<LibraryDbContext>(options =>
                options.UseInMemoryDatabase("LibraryDatabase"));

            services.AddControllersWithViews()
                    .AddNewtonsoftJson()
                    .AddRazorRuntimeCompilation();
            services.AddSwaggerDocument();

            services.AddAutoMapper(typeof(MappingProfile));

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
            app.UseAuthorization();

            app.UseOpenApi();
            app.UseSwaggerUi();

            app.UseEndpoints(endpoints =>

            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var scope = app.ApplicationServices.CreateScope())

            {
                var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                DbInitializer.Initialize(dbContext);
            }
        }
    }
}
