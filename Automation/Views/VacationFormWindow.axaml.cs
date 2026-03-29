using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class VacationFormWindow : Window
{
    private readonly VacationController _controller;
    private readonly EmployeeController _employeeController;
    
    public VacationFormWindow()
    {
        InitializeComponent();
        
        var context = DatabaseService.GetContext();
        _controller = new VacationController(context);
        _employeeController = new EmployeeController(context);
        
        LoadData();
    }
    
    private void LoadData()
    {
        CmbEmployee.ItemsSource = _employeeController.GetActive();
        CmbType.ItemsSource = Enum.GetValues(typeof(VacationType));
        CmbType.SelectedIndex = 0;
        DateStart.SelectedDate = DateTime.Now;
        DateEnd.SelectedDate = DateTime.Now.AddDays(14);
    }
    
    private void OnSave(object? sender, RoutedEventArgs e)
    {
        if (CmbEmployee.SelectedItem == null || 
            !DateStart.SelectedDate.HasValue || 
            !DateEnd.SelectedDate.HasValue)
            return;
        
        try
        {
            var vacation = new Vacation
            {
                EmployeeId = ((Employee)CmbEmployee.SelectedItem).Id,
                Type = (VacationType)CmbType.SelectedItem!,
                StartDate = DateTime.SpecifyKind(DateStart.SelectedDate.Value.DateTime, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(DateEnd.SelectedDate.Value.DateTime, DateTimeKind.Utc),
                Reason = TxtReason.Text,
                Status = VacationStatus.Pending
            };
            
            _controller.Add(vacation);
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
