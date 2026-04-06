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

public partial class EmployeeListView : UserControl
{
    private ObservableCollection<Employee> _employees = new();
    private List<Employee> _allEmployees = new();
    
    public EmployeeListView()
    {
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("EmployeeListView: Инициализация...");
        
        EmployeesGrid.ItemsSource = _employees;
        
        this.Loaded += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine("EmployeeListView: Loaded event");
            LoadFilters();
            LoadData();
        };
    }
    
    private void LoadFilters()
    {
        using var context = DatabaseService.GetContext();
        
        // Загрузка отделов
        var departments = new DepartmentController(context).GetAll();
        var deptItems = new System.Collections.Generic.List<ComboBoxItem>
        {
            new ComboBoxItem { Content = "Все отделы", Tag = null }
        };
        foreach (var dept in departments)
        {
            deptItems.Add(new ComboBoxItem { Content = dept.Name, Tag = dept.Id });
        }
        DepartmentFilter.ItemsSource = deptItems;
        DepartmentFilter.SelectedIndex = 0;
        
        // Загрузка должностей
        var positions = new PositionController(context).GetAll();
        var posItems = new System.Collections.Generic.List<ComboBoxItem>
        {
            new ComboBoxItem { Content = "Все должности", Tag = null }
        };
        foreach (var pos in positions)
        {
            posItems.Add(new ComboBoxItem { Content = pos.Name, Tag = pos.Id });
        }
        PositionFilter.ItemsSource = posItems;
        PositionFilter.SelectedIndex = 0;
    }
    
    private void LoadData()
    {
        try
        {
            using var context = DatabaseService.GetContext();
            var controller = new EmployeeController(context);
            _allEmployees = controller.GetActive();
            System.Diagnostics.Debug.WriteLine($"Загружено сотрудников: {_allEmployees.Count}");
            
            ApplyFiltersAndSort();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            ShowError($"Ошибка загрузки данных: {ex.Message}");
        }
    }
    
    private void ApplyFiltersAndSort()
    {
        var filtered = _allEmployees.AsEnumerable();
        
        // Фильтр по поиску
        var query = SearchBox.Text?.Trim();
        if (!string.IsNullOrEmpty(query))
        {
            filtered = filtered.Where(e => 
                e.FullName.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                (e.Email != null && e.Email.Contains(query, StringComparison.OrdinalIgnoreCase)));
        }
        
        // Фильтр по отделу
        if (DepartmentFilter.SelectedItem is ComboBoxItem deptItem && deptItem.Tag != null)
        {
            var deptId = (int)deptItem.Tag;
            filtered = filtered.Where(e => e.DepartmentId == deptId);
        }
        
        // Фильтр по должности
        if (PositionFilter.SelectedItem is ComboBoxItem posItem && posItem.Tag != null)
        {
            var posId = (int)posItem.Tag;
            filtered = filtered.Where(e => e.PositionId == posId);
        }
        
        // Сортировка
        var sortIndex = SortComboBox.SelectedIndex;
        filtered = sortIndex switch
        {
            0 => filtered.OrderBy(e => e.LastName),
            1 => filtered.OrderByDescending(e => e.LastName),
            2 => filtered.OrderByDescending(e => e.HireDate),
            3 => filtered.OrderBy(e => e.HireDate),
            4 => filtered.OrderByDescending(e => e.Age),
            5 => filtered.OrderBy(e => e.Age),
            _ => filtered.OrderBy(e => e.LastName)
        };
        
        _employees.Clear();
        foreach (var emp in filtered)
        {
            _employees.Add(emp);
        }
        
        System.Diagnostics.Debug.WriteLine($"Отфильтровано: {_employees.Count} из {_allEmployees.Count}");
    }
    
    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        ApplyFiltersAndSort();
    }
    
    private void OnFilterChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_allEmployees.Count > 0)
        {
            ApplyFiltersAndSort();
        }
    }
    
    private void OnSortChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (_allEmployees.Count > 0)
        {
            ApplyFiltersAndSort();
        }
    }
    
    private void OnSearch(object? sender, RoutedEventArgs e)
    {
        ApplyFiltersAndSort();
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        // HR Manager может добавлять сотрудников
        var window = new EmployeeFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private async void OnView(object? sender, RoutedEventArgs e)
    {
        var selected = EmployeesGrid.SelectedItem as Employee;
        if (selected != null)
        {
            var window = new EmployeeDetailsWindow(selected.Id);
            await window.ShowDialog((Window)this.VisualRoot!);
        }
    }
    
    private async void OnEdit(object? sender, RoutedEventArgs e)
    {
        // HR Manager может редактировать сотрудников
        var selected = EmployeesGrid.SelectedItem as Employee;
        if (selected != null)
        {
            var window = new EmployeeFormWindow(selected.Id);
            await window.ShowDialog((Window)this.VisualRoot!);
            LoadData();
        }
    }
    
    private async void OnDelete(object? sender, RoutedEventArgs e)
    {
        // Только Admin может увольнять
        if (!AuthService.IsAdmin)
        {
            await ShowMessageDialog("Доступ запрещен", "Только администратор может увольнять сотрудников");
            return;
        }
        
        var selected = EmployeesGrid.SelectedItem as Employee;
        if (selected != null)
        {
            var result = await ShowConfirmDialog($"Уволить сотрудника {selected.FullName}?");
            if (result)
            {
                using var context = DatabaseService.GetContext();
                var controller = new EmployeeController(context);
                controller.Delete(selected.Id);
                LoadData();
            }
        }
    }
    
    private async System.Threading.Tasks.Task<bool> ShowConfirmDialog(string message)
    {
        var dialog = new Window
        {
            Title = "Подтверждение",
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        
        var result = false;
        var panel = new StackPanel { Margin = new Avalonia.Thickness(20) };
        panel.Children.Add(new TextBlock { Text = message, Margin = new Avalonia.Thickness(0, 0, 0, 20) });
        
        var buttons = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right };
        var btnYes = new Button { Content = "Да", Width = 80, Margin = new Avalonia.Thickness(0, 0, 10, 0) };
        var btnNo = new Button { Content = "Нет", Width = 80 };
        
        btnYes.Click += (s, e) => { result = true; dialog.Close(); };
        btnNo.Click += (s, e) => { dialog.Close(); };
        
        buttons.Children.Add(btnYes);
        buttons.Children.Add(btnNo);
        panel.Children.Add(buttons);
        
        dialog.Content = panel;
        await dialog.ShowDialog((Window)this.VisualRoot!);
        
        return result;
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
    
    private void ShowError(string message)
    {
        var textBlock = new TextBlock
        {
            Text = message,
            Foreground = Avalonia.Media.Brushes.Red,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
        
        var parent = this.Parent as Panel;
        if (parent != null)
        {
            parent.Children.Add(textBlock);
        }
    }
}
