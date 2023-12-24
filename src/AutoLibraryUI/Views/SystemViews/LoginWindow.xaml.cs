using System;
using System.Windows;
using System.Windows.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace AutoLibraryUI.Views;

public partial class LoginWindow : Window
{
    public event Action<string?, string?>? Login;
    
    public LoginWindow()
    {
        InitializeComponent();
    }

    private bool IsInputEmpty()
    {
        return UsernameTxt.Text.Trim().Length == 0 || PasswordTxt.Password.Trim().Length == 0;
    }

    private void OnLogin()
    {
        var username = UsernameTxt.Text;
        var password = PasswordTxt.Password;

        Login?.Invoke(username, password);

        PasswordTxt.Clear();
    }

    

    protected override void OnKeyDown(KeyEventArgs e)
    {
        if(IsInputEmpty())
        {
            return;
        }

        if (e.Key == Key.Enter)
        {
            OnLogin();
        }

        base.OnKeyDown(e);
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        bool useAutoSystemLogin = false;

        if (useAutoSystemLogin)
        {
            Login?.Invoke("System", "System");
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void LoginAsReaderButton_OnClick(object sender, RoutedEventArgs e)
    {
        Login?.Invoke(null, null);
    }

    private void LoginButton_OnClick(object sender, RoutedEventArgs e)
    {
        if(IsInputEmpty())
        {
            return;
        }

        OnLogin();
    }
}