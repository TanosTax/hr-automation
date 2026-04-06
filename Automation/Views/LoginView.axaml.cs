using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Services;

namespace Automation.Views;

public partial class LoginView : UserControl
{
    public event EventHandler? LoginSuccessful;
    
    public LoginView()
    {
        InitializeComponent();
    }
    private void OnLogin(object? sender, RoutedEventArgs e)
    {
        var username = UsernameBox.Text?.Trim();
        var password = PasswordBox.Text;
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowError("Введите логин и пароль");
            return;
        }
        
        try
        {
            using var context = DatabaseService.GetContext();
            var users = context.Users.ToList();
            var user = users.FirstOrDefault(u => 
                u.Username == username && 
                u.Password == password && 
                u.IsActive);
            
            if (user != null)
            {
                AuthService.Login(user);
                LoginSuccessful?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowError("Неверный логин или пароль");
            }
        }
        catch (Exception ex)
        {
            ShowError($"Ошибка входа: {ex.Message}");
        }
    }
    
    private void ShowError(string message)
    {
        ErrorText.Text = message;
        ErrorText.IsVisible = true;
    }
}
