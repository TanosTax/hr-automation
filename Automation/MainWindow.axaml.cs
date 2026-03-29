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
        ContentArea.Content = new SalaryView();
    }
    
    private void ShowSchedule(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new WorkScheduleView();
    }
    
    private void ShowReports(object? sender, RoutedEventArgs e)
    {
        ContentArea.Content = new ReportView();
    }
}
