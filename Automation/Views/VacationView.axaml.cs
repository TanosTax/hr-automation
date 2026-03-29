using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class VacationView : UserControl
{
    private readonly VacationController _controller;
    
    public VacationView()
    {
        InitializeComponent();
        _controller = new VacationController(DatabaseService.GetContext());
        LoadData();
    }
    
    private void LoadData()
    {
        var vacations = _controller.GetAll();
        VacationsGrid.ItemsSource = vacations;
    }
    
    private void OnShowAll(object? sender, RoutedEventArgs e)
    {
        LoadData();
    }
    
    private void OnShowPending(object? sender, RoutedEventArgs e)
    {
        var pending = _controller.GetPending();
        VacationsGrid.ItemsSource = pending;
    }
    
    private void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new VacationFormWindow();
        window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnApprove(object? sender, RoutedEventArgs e)
    {
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            _controller.Approve(selected.Id);
            LoadData();
        }
    }
    
    private void OnReject(object? sender, RoutedEventArgs e)
    {
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            _controller.Reject(selected.Id, "Отклонено");
            LoadData();
        }
    }
}
