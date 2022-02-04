using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Store.Models;
using Store.Models.Repositories.Abstract;
using Store.Models.Repositories.EntityFramework;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddTransient<IProductRepository, EFProductRepository>();


var app = builder.Build();
app.Configuration.Bind("Project", new Config());

builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(Config.ConnectionString);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();
app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=ShowListProducts}/{id?}");

app.Run();
