using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using RPS.Network.Models;
using RPS.Network.Services;
using TodoApp.Services;
using TodoDB.Todos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<TodoDbSettings>(
    builder.Configuration.GetSection("TodoDBDatabase"));

builder.Services.AddSingleton<TodosService>();

builder.Services.Configure<RPSDBSettings>(
    builder.Configuration.GetSection("RPSDBDatabase"));

builder.Services.AddSingleton<RPSService>();

// Configure Kestrel for running in docker
//builder.WebHost.ConfigureKestrel(serverOptions =>
//{
//    // Listen on port 5166 on any IP address
//    serverOptions.ListenAnyIP(5166);
//});

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
