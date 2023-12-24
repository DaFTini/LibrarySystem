using AutoLibraryUI.Data;
using AutoLibraryUI.Entities;
using AutoLibraryUI.Interfaces;
using AutoLibraryUI.Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AutoLibraryUI.Extensions;

public static class DataExtensions
{
    public static void SeedData(this IServiceProvider services)
    {
        var context = services.GetRequiredService<LibraryDbContext>();
        var authService = services.GetRequiredService<IAuthService<Librarian>>();
        var accountsService = services.GetRequiredService<RegistationList>();

        // context.Database.EnsureDeleted();

        if(context.Database.EnsureCreated())
        {
            authService.Register(new Librarian
            {
                Username = "System",
                FullName = "System",
                Role = "Администратор"
            }, password: "System");
        }
    }
}