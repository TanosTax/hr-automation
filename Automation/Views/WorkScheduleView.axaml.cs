using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Services;

namespace Automation.Views;

public partial class WorkScheduleView : UserControl
{
    private readonly WorkScheduleController _controller;
    
    public WorkScheduleView()
    {
        InitializeComponent();
        _controller = new WorkScheduleController(DatabaseService.GetContext());
        DatePicker.SelectedDate = DateTime.Now;
    }
    
    private void OnLoad(object? sender, RoutedEventArgs e)
    {
        if (DatePicker.SelectedDate.HasValue)
        {
            var schedules = _controller.GetByDate(DatePicker.SelectedDate.Value.DateTime);
            ScheduleGrid.ItemsSource = schedules;
        }
    }
    
    private void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new WorkScheduleFormWindow();
        window.ShowDialog((Window)this.VisualRoot!);
        OnLoad(sender, e);
    }
}
