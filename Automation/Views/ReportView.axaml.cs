using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Services;

namespace Automation.Views;

public partial class ReportView : UserControl
{
    private readonly EmployeeController _employeeController;
    private readonly DepartmentController _departmentController;
    private readonly SalaryController _salaryController;
    private readonly VacationController _vacationController;
    
    public ReportView()
    {
        InitializeComponent();
        
        var context = DatabaseService.GetContext();
        _employeeController = new EmployeeController(context);
        _departmentController = new DepartmentController(context);
        _salaryController = new SalaryController(context);
        _vacationController = new VacationController(context);
    }
    
    private void OnEmployeeReport(object? sender, RoutedEventArgs e)
    {
        var employees = _employeeController.GetActive();
        
        var grid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All
        };
        
        grid.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new Avalonia.Data.Binding("FullName"), Width = new DataGridLength(250) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Отдел", Binding = new Avalonia.Data.Binding("Department.Name"), Width = new DataGridLength(150) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Должность", Binding = new Avalonia.Data.Binding("Position.Name"), Width = new DataGridLength(150) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Телефон", Binding = new Avalonia.Data.Binding("Phone"), Width = new DataGridLength(120) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Email", Binding = new Avalonia.Data.Binding("Email"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        
        grid.ItemsSource = employees;
        
        ReportContent.Children.Clear();
        ReportContent.Children.Add(new TextBlock { Text = $"Всего сотрудников: {employees.Count}", FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 0, 0, 10) });
        ReportContent.Children.Add(grid);
    }
    
    private void OnDepartmentReport(object? sender, RoutedEventArgs e)
    {
        var departments = _departmentController.GetAll();
        
        var grid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All
        };
        
        grid.Columns.Add(new DataGridTextColumn { Header = "Отдел", Binding = new Avalonia.Data.Binding("Name"), Width = new DataGridLength(250) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Сотрудников", Binding = new Avalonia.Data.Binding("Employees.Count"), Width = new DataGridLength(120) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Описание", Binding = new Avalonia.Data.Binding("Description"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        
        grid.ItemsSource = departments;
        
        ReportContent.Children.Clear();
        ReportContent.Children.Add(new TextBlock { Text = "Статистика по отделам", FontSize = 18, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 0, 0, 10) });
        ReportContent.Children.Add(grid);
    }
    
    private void OnSalaryReport(object? sender, RoutedEventArgs e)
    {
        var year = DateTime.Now.Year;
        var month = DateTime.Now.Month;
        var salaries = _salaryController.GetByPeriod(year, month);
        
        var grid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All
        };
        
        grid.Columns.Add(new DataGridTextColumn { Header = "Сотрудник", Binding = new Avalonia.Data.Binding("Employee.FullName"), Width = new DataGridLength(200) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Отдел", Binding = new Avalonia.Data.Binding("Employee.Department.Name"), Width = new DataGridLength(150) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Оклад", Binding = new Avalonia.Data.Binding("BaseSalary") { StringFormat = "{0:N2}" }, Width = new DataGridLength(100) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Премия", Binding = new Avalonia.Data.Binding("Bonus") { StringFormat = "{0:N2}" }, Width = new DataGridLength(100) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Итого", Binding = new Avalonia.Data.Binding("Total") { StringFormat = "{0:N2}" }, Width = new DataGridLength(120) });
        
        grid.ItemsSource = salaries;
        
        var total = salaries.Sum(s => s.Total);
        
        ReportContent.Children.Clear();
        ReportContent.Children.Add(new TextBlock { Text = $"Зарплатная ведомость за {month:D2}.{year}", FontSize = 18, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 0, 0, 10) });
        ReportContent.Children.Add(grid);
        ReportContent.Children.Add(new TextBlock { Text = $"Общая сумма: {total:N2} ₽", FontSize = 16, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 10, 0, 0) });
    }
    
    private void OnVacationReport(object? sender, RoutedEventArgs e)
    {
        var vacations = _vacationController.GetAll().Take(50).ToList();
        
        var grid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All
        };
        
        grid.Columns.Add(new DataGridTextColumn { Header = "Сотрудник", Binding = new Avalonia.Data.Binding("Employee.FullName"), Width = new DataGridLength(200) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Тип", Binding = new Avalonia.Data.Binding("Type"), Width = new DataGridLength(120) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Начало", Binding = new Avalonia.Data.Binding("StartDate") { StringFormat = "{0:dd.MM.yyyy}" }, Width = new DataGridLength(100) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Конец", Binding = new Avalonia.Data.Binding("EndDate") { StringFormat = "{0:dd.MM.yyyy}" }, Width = new DataGridLength(100) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Статус", Binding = new Avalonia.Data.Binding("Status"), Width = new DataGridLength(120) });
        
        grid.ItemsSource = vacations;
        
        ReportContent.Children.Clear();
        ReportContent.Children.Add(new TextBlock { Text = "Отчет по отпускам", FontSize = 18, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 0, 0, 10) });
        ReportContent.Children.Add(grid);
    }
    
    private void OnBirthdayReport(object? sender, RoutedEventArgs e)
    {
        var employees = _employeeController.GetActive()
            .Where(e => e.BirthDate.Month == DateTime.Now.Month)
            .OrderBy(e => e.BirthDate.Day)
            .ToList();
        
        var grid = new DataGrid
        {
            AutoGenerateColumns = false,
            IsReadOnly = true,
            GridLinesVisibility = DataGridGridLinesVisibility.All
        };
        
        grid.Columns.Add(new DataGridTextColumn { Header = "ФИО", Binding = new Avalonia.Data.Binding("FullName"), Width = new DataGridLength(250) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Дата рождения", Binding = new Avalonia.Data.Binding("BirthDate") { StringFormat = "{0:dd.MM.yyyy}" }, Width = new DataGridLength(150) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Отдел", Binding = new Avalonia.Data.Binding("Department.Name"), Width = new DataGridLength(150) });
        grid.Columns.Add(new DataGridTextColumn { Header = "Должность", Binding = new Avalonia.Data.Binding("Position.Name"), Width = new DataGridLength(1, DataGridLengthUnitType.Star) });
        
        grid.ItemsSource = employees;
        
        ReportContent.Children.Clear();
        ReportContent.Children.Add(new TextBlock { Text = $"Дни рождения в {DateTime.Now:MMMM}", FontSize = 18, FontWeight = Avalonia.Media.FontWeight.Bold, Margin = new Avalonia.Thickness(0, 0, 0, 10) });
        ReportContent.Children.Add(grid);
    }
    
    private async void OnExport(object? sender, RoutedEventArgs e)
    {
        var dialog = new Window
        {
            Title = "Экспорт",
            Width = 300,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = "Функция экспорта в разработке", Margin = new Avalonia.Thickness(0, 0, 0, 20) },
                    new Button { Content = "OK", Width = 80, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right }
                }
            }
        };
        
        var btn = ((StackPanel)dialog.Content).Children[1] as Button;
        btn!.Click += (s, e) => dialog.Close();
        
        await dialog.ShowDialog((Window)this.VisualRoot!);
    }
}
