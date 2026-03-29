using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class WorkScheduleFormWindow : Window
{
    private readonly WorkScheduleController _controller;
    private readonly EmployeeController _employeeController;
    
    public WorkScheduleFormWindow()
    {
        InitializeComponent();
        
        var context = DatabaseService.GetContext();
        _controller = new WorkScheduleController(context);
        _employeeController = new EmployeeController(context);
        
        CmbEmployee.ItemsSource = _employeeController.GetActive();
        DatePicker.SelectedDate = DateTime.Now;
        TimeCheckIn.SelectedTime = new TimeSpan(9, 0, 0);
        TimeCheckOut.SelectedTime = new TimeSpan(18, 0, 0);
    }
    
    private void OnSave(object? sender, RoutedEventArgs e)
    {
        if (CmbEmployee.SelectedItem == null || !DatePicker.SelectedDate.HasValue)
            return;
        
        try
        {
            var schedule = new WorkSchedule
            {
                EmployeeId = ((Employee)CmbEmployee.SelectedItem).Id,
                Date = DateTime.SpecifyKind(DatePicker.SelectedDate.Value.DateTime, DateTimeKind.Utc),
                CheckIn = TimeCheckIn.SelectedTime,
                CheckOut = TimeCheckOut.SelectedTime,
                IsAbsent = ChkAbsent.IsChecked ?? false,
                AbsenceReason = TxtReason.Text
            };
            
            _controller.Add(schedule);
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
