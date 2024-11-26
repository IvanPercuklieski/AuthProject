using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IBLab.Data;
using IBLab.Service.impl;
using IBLab.Service.interfaces;
using IBLab.Repository.Interfaces;
using IBLab.Repository.Impl;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<IBLabContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IBLabContext") ?? throw new InvalidOperationException("Connection string 'IBLabContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserRepository, UserRepositoryImpl>();
builder.Services.AddScoped<IUserService, UserServiceImpl>();

builder.Services.AddScoped<ITempUserRepository, TempUserRepositoryImpl>(); //TODO: da ne e repo tuka :/

builder.Services.AddHostedService<CleanupService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Users}/{action=Register}/{id?}");

app.Run();




