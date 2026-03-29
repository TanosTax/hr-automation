using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class VacationView : UserControl
{
    private ObservableCollection<Vacation> _vacations = new();
    
    public VacationView()
    {
        InitializeComponent();
        
        VacationsGrid.ItemsSource = _vacations;
        
        this.Loaded += (s, e) =>
        {
            LoadData();
        };
    }
    
    private void LoadData()
    {
        using var context = DatabaseService.GetContext();
        var controller = new VacationController(context);
        var vacations = controller.GetAll();
        _vacations.Clear();
        foreach (var vac in vacations)
        {
            _vacations.Add(vac);
        }
    }
    
    private void OnShowAll(object? sender, RoutedEventArgs e)
    {
        LoadData();
    }
    
    private void OnShowPending(object? sender, RoutedEventArgs e)
    {
        using var context = DatabaseService.GetContext();
        var controller = new VacationController(context);
        var pending = controller.GetPending();
        _vacations.Clear();
        foreach (var vac in pending)
        {
            _vacations.Add(vac);
        }
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new VacationFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnApprove(object? sender, RoutedEventArgs e)
    {
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            using var context = DatabaseService.GetContext();
            var controller = new VacationController(context);
            controller.Approve(selected.Id);
            LoadData();
        }
    }
    
    private void OnReject(object? sender, RoutedEventArgs e)
    {
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            using var context = DatabaseService.GetContext();
            var controller = new VacationController(context);
            controller.Reject(selected.Id, "Отклонено");
            LoadData();
        }
    }
}
