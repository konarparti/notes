using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Models;
using Store.Models.Repositories;
using Store.Models.Repositories.Abstract;
using Store.Models.Repositories.EntityFramework;

namespace WebMVC
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("Project", new Config());
            services.AddTransient<IProductRepository, EFProductRepository>();
            
            services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(Config.ConnectionString));

            services.AddMvc();   

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "{category}/Page{productPage:int}",
                    defaults: new { controller = "Product", action = "ShowListProducts" }
                    );

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "Page{productPage:int}",
                    defaults: new { controller = "Product", action = "ShowListProducts", productPage = 1 }
                    );

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "category",
                    defaults: new { controller = "Product", action = "ShowListProducts", productPage = 1 }
                    );

                endpoints.MapControllerRoute(
                    name: null,
                    pattern: "",
                    defaults: new { controller = "Product", action = "ShowListProducts", productPage = 1 }
                    );

                endpoints.MapControllerRoute(
                   name: null,
                   pattern: "{controller}/{action}/{id?}"
                   );               
            });
        }
    }
}