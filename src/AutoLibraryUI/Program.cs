using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Windows;
using AutoLibraryUI.Data;
using AutoLibraryUI.Entities;
using AutoLibraryUI.Extensions;
using AutoLibraryUI.Interfaces;
using AutoLibraryUI.Services;
using AutoLibraryUI.Views;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutoLibraryUI;

public static class Program
{
    [STAThread]
    public static int Main(string[] args)
    {
        const string connectionString = "Host=localhost;Port=5432;Database=library_db;Username=postgres;Password=2404";
        
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddSingleton<Application, App>();
                
                services.AddDbContext<LibraryDbContext>(options =>
                {
                    options.UseNpgsql(connectionString);
                }, ServiceLifetime.Singleton);
                
                services.AddSingleton<IAuthService<Librarian>, RegistationList>();
                services.AddSingleton<PasswordHasher<Librarian>>();
                
                services.AddSingleton<RegistationList>();
                services.AddSingleton<CatalogService>();
                
                services.AddTransient<LoginWindow>();
                services.AddSingleton<NavigationWindow>();
            })
            .Build();
        
        host.Services.SeedData();

        var app = host.Services.GetRequiredService<Application>();

        return app.Run();
    }
}