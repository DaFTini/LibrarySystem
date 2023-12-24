using AutoLibraryUI.Entities;
using AutoLibraryUI.Interfaces;
using AutoLibraryUI.Views;
using System;
using System.Security.Principal;
using System.Windows;

namespace AutoLibraryUI;

public class App : Application
{ 
    private readonly LoginWindow _loginWindow;
    private readonly NavigationWindow _navigationWindow;
    private readonly IAuthService<Librarian> _authService;
    
    public App(LoginWindow loginWindow, NavigationWindow navigationWindow, IAuthService<Librarian> authService)
    {
        Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;

        _navigationWindow = navigationWindow ?? throw new ArgumentNullException(nameof(navigationWindow));
        _loginWindow = loginWindow ?? throw new ArgumentNullException(nameof(loginWindow));
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        _loginWindow.Login += (username, password) =>
        {
            if(username == null || password == null)
            {
                AnonimousLogin();
            }
            else
            {
                IdentityLogin(username, password);
            }       
        };
    }
    
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        _loginWindow.Show();
    }

    private void AnonimousLogin()
    {
        System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(
                        new GenericIdentity("Анонимный пользователь"),
                        new string[] { Roles.Reader });

        _loginWindow.Visibility = Visibility.Hidden;
        _navigationWindow.Show();
    }

    private void IdentityLogin(string username, string password)
    {
        var user = _authService.Login(username, password);

        if(user != null)
        {
            GenericIdentity identity = new GenericIdentity(username);

            System.Threading.Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[] { user.Role });
        }

        if(user == null)
        {
            MessageBox.Show("Неверное имя пользователя или пароль");
        }
        else
        {
            _loginWindow.Visibility = Visibility.Hidden;
            _navigationWindow.Show();
        }
    }
}