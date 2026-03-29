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
            TxtPosition.Text = employee.Position.Name;
            TxtEmail.Text = employee.Email ?? "-";
            TxtPhone.Text = employee.Phone ?? "-";
            TxtDepartment.Text = employee.Department.Name;
            TxtBirthDate.Text = employee.BirthDate.ToString("dd.MM.yyyy");
            TxtHireDate.Text = employee.HireDate.ToString("dd.MM.yyyy");
            TxtAddress.Text = employee.Address ?? "-";
            
            ContractsGrid.ItemsSource = employee.Contracts;
            VacationsGrid.ItemsSource = employee.Vacations;
            DocumentsGrid.ItemsSource = employee.Documents;
        }
    }
    
    private void OnClose(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
