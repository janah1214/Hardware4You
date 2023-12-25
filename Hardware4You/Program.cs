using Hardware4You.Data;
using Hardware4You.Data.User;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductCategoryService, ProductCategoryService>();
builder.Services.AddScoped<CartService>();

builder.Services.AddDbContext<ProductContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("HardwareDatabase")));

builder.Services.AddDbContext<ProductCategoryContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("HardwareDatabase")));

builder.Services.AddDbContext<UserContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("HardwareDatabase")));

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<UserService>());

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSyncfusionBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
