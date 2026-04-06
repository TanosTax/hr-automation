using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class VacationView : UserControl
{
    private ObservableCollection<Vacation> _vacations = new();
    private List<Vacation> _allVacations = new();
    
    public VacationView()
    {
        InitializeComponent();
        
        VacationsGrid.ItemsSource = _vacations;
        
        this.Loaded += (s, e) =>
        {
            LoadData();
        };
        
        // Только Admin может одобрять/отклонять
        if (!AuthService.IsAdmin)
        {
            BtnApprove.IsEnabled = false;
            BtnReject.IsEnabled = false;
        }
    }
    
    private void LoadData()
    {
        using var context = DatabaseService.GetContext();
        var controller = new VacationController(context);
        _allVacations = controller.GetAll();
        ApplyFiltersAndSort();
    }
    
    private void ApplyFiltersAndSort()
    {
        var filtered = _allVacations.AsEnumerable();
        
        // Фильтр по статусу
        var statusIndex = StatusFilter.SelectedIndex;
        filtered = statusIndex switch
        {
            1 => filtered.Where(v => v.Status == VacationStatus.Pending),
            2 => filtered.Where(v => v.Status == VacationStatus.Approved),
            3 => filtered.Where(v => v.Status == VacationStatus.Rejected),
            _ => filtered
        };
        
        // Фильтр по типу
        var typeIndex = TypeFilter.SelectedIndex;
        filtered = typeIndex switch
        {
            1 => filtered.Where(v => v.Type == VacationType.Vacation),
            2 => filtered.Where(v => v.Type == VacationType.SickLeave),
            3 => filtered.Where(v => v.Type == VacationType.Unpaid),
            _ => filtered
        };
        
        // Сортировка
        var sortIndex = SortComboBox.SelectedIndex;
        filtered = sortIndex switch
        {
            0 => filtered.OrderByDescending(v => v.StartDate),
            1 => filtered.OrderBy(v => v.StartDate),
            2 => filtered.OrderByDescending(v => v.DaysCount),
            3 => filtered.OrderBy(v => v.DaysCount),
            4 => filtered.OrderBy(v => v.Employee.LastName),
            5 => filtered.OrderByDescending(v => v.Employee.LastName),
            _ => filtered.OrderByDescending(v => v.StartDate)
        };
        
        _vacations.Clear();
        foreach (var vac in filtered)
        {
            _vacations.Add(vac);
        }
    }
    
    private void OnFilterChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_allVacations.Count > 0)
        {
            ApplyFiltersAndSort();
        }
    }
    
    private void OnSortChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_allVacations.Count > 0)
        {
            ApplyFiltersAndSort();
        }
    }
    
    private void OnShowAll(object? sender, RoutedEventArgs e)
    {
        StatusFilter.SelectedIndex = 0;
        TypeFilter.SelectedIndex = 0;
        LoadData();
    }
    
    private void OnShowPending(object? sender, RoutedEventArgs e)
    {
        StatusFilter.SelectedIndex = 1;
        ApplyFiltersAndSort();
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        // HR Manager может добавлять отпуска
        var window = new VacationFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private async void OnApprove(object? sender, RoutedEventArgs e)
    {
        if (!AuthService.IsAdmin)
        {
            await ShowMessageDialog("Доступ запрещен", "Только администратор может одобрять отпуска");
            return;
        }
        
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            using var context = DatabaseService.GetContext();
            var controller = new VacationController(context);
            controller.Approve(selected.Id);
            LoadData();
        }
    }
    
    private async void OnReject(object? sender, RoutedEventArgs e)
    {
        if (!AuthService.IsAdmin)
        {
            await ShowMessageDialog("Доступ запрещен", "Только администратор может отклонять отпуска");
            return;
        }
        
        var selected = VacationsGrid.SelectedItem as Vacation;
        if (selected != null && selected.Status == VacationStatus.Pending)
        {
            using var context = DatabaseService.GetContext();
            var controller = new VacationController(context);
            controller.Reject(selected.Id, "Отклонено");
            LoadData();
        }
    }
    
    private async System.Threading.Tasks.Task ShowMessageDialog(string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        
        var panel = new StackPanel { Margin = new Avalonia.Thickness(20) };
        panel.Children.Add(new TextBlock { Text = message, Margin = new Avalonia.Thickness(0, 0, 0, 20), TextWrapping = Avalonia.Media.TextWrapping.Wrap });
        
        var btnOk = new Button { Content = "OK", Width = 80, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
        btnOk.Click += (s, e) => { dialog.Close(); };
        
        panel.Children.Add(btnOk);
        dialog.Content = panel;
        await dialog.ShowDialog((Window)this.VisualRoot!);
    }
}
