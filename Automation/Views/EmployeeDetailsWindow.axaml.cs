using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Services;

namespace Automation.Views;

public partial class EmployeeDetailsWindow : Window
{
    private readonly EmployeeController _controller;
    private readonly int _employeeId;
    
    public EmployeeDetailsWindow(int employeeId)
    {
        InitializeComponent();
        _controller = new EmployeeController(DatabaseService.GetContext());
        _employeeId = employeeId;
        LoadData();
    }
    
    private void LoadData()
    {
        var employee = _controller.GetById(_employeeId);
        if (employee != null)
        {
            TxtFullName.Text = employee.FullName;
            TxtBirthDate.Text = employee.BirthDate.ToString("dd.MM.yyyy");
            TxtPassport.Text = $"{employee.PassportSeries} {employee.PassportNumber}";
            TxtAddress.Text = employee.Address ?? "-";
            TxtPhone.Text = employee.Phone ?? "-";
            TxtEmail.Text = employee.Email ?? "-";
            TxtDepartment.Text = employee.Department.Name;
            TxtPosition.Text = employee.Position.Name;
            TxtHireDate.Text = employee.HireDate.ToString("dd.MM.yyyy");
            TxtStatus.Text = employee.IsActive ? "Работает" : $"Уволен ({employee.FireDate:dd.MM.yyyy})";
            
            DocumentsGrid.ItemsSource = employee.Documents;
            ContractsGrid.ItemsSource = employee.Contracts;
            VacationsGrid.ItemsSource = employee.Vacations;
        }
    }
    
    private void OnClose(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
