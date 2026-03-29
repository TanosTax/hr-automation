using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class EmployeeFormWindow : Window
{
    private readonly EmployeeController _employeeController;
    private readonly DepartmentController _departmentController;
    private readonly PositionController _positionController;
    private readonly int? _employeeId;
    
    public EmployeeFormWindow(int? employeeId = null)
    {
        InitializeComponent();
        
        var context = DatabaseService.GetContext();
        _employeeController = new EmployeeController(context);
        _departmentController = new DepartmentController(context);
        _positionController = new PositionController(context);
        _employeeId = employeeId;
        
        LoadDictionaries();
        
        if (_employeeId.HasValue)
        {
            LoadEmployee(_employeeId.Value);
            Title = "Редактирование сотрудника";
        }
        else
        {
            Title = "Новый сотрудник";
            DateBirth.SelectedDate = DateTime.Now.AddYears(-25);
            DateHire.SelectedDate = DateTime.Now;
        }
    }
    
    private void LoadDictionaries()
    {
        CmbDepartment.ItemsSource = _departmentController.GetAll();
        CmbPosition.ItemsSource = _positionController.GetAll();
    }
    
    private void LoadEmployee(int id)
    {
        var employee = _employeeController.GetById(id);
        if (employee != null)
        {
            TxtLastName.Text = employee.LastName;
            TxtFirstName.Text = employee.FirstName;
            TxtMiddleName.Text = employee.MiddleName;
            DateBirth.SelectedDate = employee.BirthDate;
            TxtPassportSeries.Text = employee.PassportSeries;
            TxtPassportNumber.Text = employee.PassportNumber;
            TxtAddress.Text = employee.Address;
            TxtPhone.Text = employee.Phone;
            TxtEmail.Text = employee.Email;
            DateHire.SelectedDate = employee.HireDate;
            
            CmbDepartment.SelectedItem = _departmentController.GetAll()
                .FirstOrDefault(d => d.Id == employee.DepartmentId);
            CmbPosition.SelectedItem = _positionController.GetAll()
                .FirstOrDefault(p => p.Id == employee.PositionId);
        }
    }
    
    private async void OnSave(object? sender, RoutedEventArgs e)
    {
        if (!ValidateForm())
        {
            await ShowMessage("Заполните все обязательные поля (*)");
            return;
        }
        
        try
        {
            var employee = _employeeId.HasValue 
                ? _employeeController.GetById(_employeeId.Value)!
                : new Employee();
            
            employee.LastName = TxtLastName.Text!;
            employee.FirstName = TxtFirstName.Text!;
            employee.MiddleName = TxtMiddleName.Text;
            employee.BirthDate = DateBirth.SelectedDate!.Value.DateTime;
            employee.PassportSeries = TxtPassportSeries.Text;
            employee.PassportNumber = TxtPassportNumber.Text;
            employee.Address = TxtAddress.Text;
            employee.Phone = TxtPhone.Text;
            employee.Email = TxtEmail.Text;
            employee.HireDate = DateHire.SelectedDate!.Value.DateTime;
            employee.DepartmentId = ((Department)CmbDepartment.SelectedItem!).Id;
            employee.PositionId = ((Position)CmbPosition.SelectedItem!).Id;
            
            if (_employeeId.HasValue)
                _employeeController.Update(employee);
            else
                _employeeController.Add(employee);
            
            Close();
        }
        catch (Exception ex)
        {
            await ShowMessage($"Ошибка сохранения: {ex.Message}");
        }
    }
    
    private void OnCancel(object? sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private bool ValidateForm()
    {
        return !string.IsNullOrWhiteSpace(TxtLastName.Text) &&
               !string.IsNullOrWhiteSpace(TxtFirstName.Text) &&
               DateBirth.SelectedDate.HasValue &&
               DateHire.SelectedDate.HasValue &&
               CmbDepartment.SelectedItem != null &&
               CmbPosition.SelectedItem != null;
    }
    
    private async System.Threading.Tasks.Task ShowMessage(string message)
    {
        var dialog = new Window
        {
            Title = "Сообщение",
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap, Margin = new Avalonia.Thickness(0, 0, 0, 20) },
                    new Button { Content = "OK", Width = 80, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right }
                }
            }
        };
        
        var btn = ((StackPanel)dialog.Content).Children.OfType<Button>().First();
        btn.Click += (s, e) => dialog.Close();
        
        await dialog.ShowDialog(this);
    }
}
