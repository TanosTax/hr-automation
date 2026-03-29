using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class WorkScheduleView : UserControl
{
    private ObservableCollection<WorkSchedule> _schedules = new();
    
    public WorkScheduleView()
    {
        InitializeComponent();
        
        ScheduleGrid.ItemsSource = _schedules;
        DatePicker.SelectedDate = DateTime.Now;
        
        this.Loaded += (s, e) =>
        {
            LoadAllData();
        };
    }
    
    private void LoadAllData()
    {
        using var context = DatabaseService.GetContext();
        var controller = new WorkScheduleController(context);
        var schedules = controller.GetAll();
        _schedules.Clear();
        foreach (var sch in schedules)
        {
            _schedules.Add(sch);
        }
    }
    
    private void OnLoad(object? sender, RoutedEventArgs e)
    {
        if (DatePicker.SelectedDate.HasValue)
        {
            using var context = DatabaseService.GetContext();
            var controller = new WorkScheduleController(context);
            var schedules = controller.GetByDate(DatePicker.SelectedDate.Value.DateTime);
            _schedules.Clear();
            foreach (var sch in schedules)
            {
                _schedules.Add(sch);
            }
        }
    }
    
    private void OnShowAll(object? sender, RoutedEventArgs e)
    {
        LoadAllData();
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new WorkScheduleFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        OnLoad(sender, e);
    }
}
