using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class SalaryFormWindow : Window
{
    private readonly SalaryController _controller;
    private readonly EmployeeController _employeeController;
    
    public SalaryFormWindow()
    {
        InitializeComponent();
        
        var context = DatabaseService.GetContext();
        _controller = new SalaryController(context);
        _employeeController = new EmployeeController(context);
        
        LoadData();
    }
    
    private void LoadData()
    {
        CmbEmployee.ItemsSource = _employeeController.GetActive();
        NumYear.Value = DateTime.Now.Year;
        NumMonth.Value = DateTime.Now.Month;
        NumBonus.Value = 0;
        NumDeductions.Value = 0;
    }
    
    private void OnEmployeeSelected(object? sender, SelectionChangedEventArgs e)
    {
        var employee = CmbEmployee.SelectedItem as Employee;
        if (employee != null)
        {
            NumBaseSalary.Value = employee.Position.BaseSalary;
        }
    }
    
    private void OnSave(object? sender, RoutedEventArgs e)
    {
        if (CmbEmployee.SelectedItem == null)
            return;
        
        try
        {
            var salary = new Salary
            {
                EmployeeId = ((Employee)CmbEmployee.SelectedItem).Id,
                Year = (int)(NumYear.Value ?? DateTime.Now.Year),
                Month = (int)(NumMonth.Value ?? DateTime.Now.Month),
                BaseSalary = NumBaseSalary.Value ?? 0,
                Bonus = NumBonus.Value ?? 0,
                Deductions = NumDeductions.Value ?? 0,
                Notes = TxtNotes.Text,
                PaymentDate = DateTime.UtcNow
            };
            
            _controller.Add(salary);
            Close();
        }
        catch (Exception)
        {
            // Обработка ошибок
        }
    }
    
    private void OnCancel(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
