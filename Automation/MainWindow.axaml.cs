using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Views;
using Automation.Services;

namespace Automation;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        // Инициализация БД при запуске
        try
        {
            DatabaseService.InitializeDatabase();
            ShowLoginView();
        }
        catch (System.Exception ex)
        {
            // Показываем ошибку если БД недоступна
            ContentArea.Content = new TextBlock 
            { 
                Text = $"Ошибка подключения к БД:\n{ex.Message}",
                Foreground = Avalonia.Media.Brushes.Red,
                TextWrapping = Avalonia.Media.TextWrapping.Wrap
            };
        }
    }
    
    private void ShowLoginView()
    {
        var loginView = new LoginView();
        loginView.LoginSuccessful += (s, e) =>
        {
            UpdateUIForRole();
            ShowWelcomeScreen();
        };
        ContentArea.Content = loginView;
        
        // Скрываем меню и делаем ContentArea на весь экран
        MenuPanel.IsVisible = false;
        ContentAreaBorder.SetValue(Grid.ColumnProperty, 0);
        ContentAreaBorder.SetValue(Grid.ColumnSpanProperty, 2);
    }
    
    private void ShowWelcomeScreen()
    {
        // Показываем меню и возвращаем нормальную разметку
        MenuPanel.IsVisible = true;
        ContentAreaBorder.SetValue(Grid.ColumnProperty, 1);
        ContentAreaBorder.SetValue(Grid.ColumnSpanProperty, 1);
        
        ContentArea.Content = new StackPanel 
        { 
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center, 
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
            Children =
            {
                new Border
                {
                    CornerRadius = new Avalonia.CornerRadius(24),
                    Padding = new Avalonia.Thickness(60, 50),
                    BoxShadow = Avalonia.Media.BoxShadows.Parse("0 16 48 0 #00000080"),
                    Background = new Avalonia.Media.LinearGradientBrush
                    {
                        StartPoint = new Avalonia.RelativePoint(0, 0, Avalonia.RelativeUnit.Relative),
                        EndPoint = new Avalonia.RelativePoint(1, 1, Avalonia.RelativeUnit.Relative),
                        GradientStops = new Avalonia.Media.GradientStops
                        {
                            new Avalonia.Media.GradientStop(Avalonia.Media.Color.Parse("#0E639C"), 0),
                            new Avalonia.Media.GradientStop(Avalonia.Media.Color.Parse("#1177BB"), 0.5),
                            new Avalonia.Media.GradientStop(Avalonia.Media.Color.Parse("#7C3AED"), 1)
                        }
                    },
                    Child = new StackPanel
                    {
                        Children =
                        {
                            new TextBlock { Text = "👋", FontSize = 72, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Avalonia.Thickness(0, 0, 0, 25) },
                            new TextBlock { Text = "Добро пожаловать!", FontSize = 34, FontWeight = Avalonia.Media.FontWeight.Black, Foreground = Avalonia.Media.Brushes.White, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, Margin = new Avalonia.Thickness(0, 0, 0, 12) },
                            new TextBlock { Text = "Выберите раздел в меню слева", FontSize = 17, Foreground = Avalonia.Media.Brush.Parse("#E0F0FF"), HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center, FontWeight = Avalonia.Media.FontWeight.Medium }
                        }
                    }
                }
            }
        };
    }
    
    private void UpdateUIForRole()
    {
        if (AuthService.CurrentUser != null)
        {
            UserInfoText.Text = $"{AuthService.CurrentUser.FullName} ({AuthService.CurrentUser.Role})";
            
            // HR Manager не видит зарплаты и табель
            if (AuthService.IsHRManager)
            {
                BtnSalaries.IsVisible = false;
                BtnSchedule.IsVisible = false;
            }
            else
            {
                BtnSalaries.IsVisible = true;
                BtnSchedule.IsVisible = true;
            }
        }
    }
    
    private void OnCloseApp(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void OnLogout(object? sender, RoutedEventArgs e)
    {
        AuthService.Logout();
        ShowLoginView();
    }
    
    private void ShowEmployees(object? sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("ShowEmployees вызван!");
        ContentArea.Content = new EmployeeListView();
        System.Diagnostics.Debug.WriteLine("EmployeeListView установлен в ContentArea");
    }
    
    private void ShowDepartments(object? sender, RoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("ShowDepartments вызван!");
        ContentArea.Content = new DepartmentView();
        System.Diagnostics.Debug.WriteLine("DepartmentView установлен в ContentArea");
    }
    
    private void ShowPositions(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new PositionView();
    }
    
    private void ShowVacations(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new VacationView();
    }
    
    private void ShowSalaries(object? sender, RoutedEventArgs e)
    {
        if (!AuthService.IsAdmin)
        {
            ContentArea.Content = new TextBlock 
            { 
                Text = "Доступ запрещен",
                Foreground = Avalonia.Media.Brushes.Red,
                FontSize = 24,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            return;
        }
        ContentArea.Content = new SalaryView();
    }
    
    private void ShowSchedule(object? sender, RoutedEventArgs e)
    {
        if (!AuthService.IsAdmin)
        {
            ContentArea.Content = new TextBlock 
            { 
                Text = "Доступ запрещен",
                Foreground = Avalonia.Media.Brushes.Red,
                FontSize = 24,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
            };
            return;
        }
        ContentArea.Content = new WorkScheduleView();
    }
    
    private void ShowReports(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ReportView();
    }
}
