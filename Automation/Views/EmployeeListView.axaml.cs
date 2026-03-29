using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class EmployeeListView : UserControl
{
    private ObservableCollection<Employee> _employees = new();
    
    public EmployeeListView()
    {
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("EmployeeListView: Инициализация...");
        
        EmployeesGrid.ItemsSource = _employees;
        
        this.Loaded += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine("EmployeeListView: Loaded event");
            LoadData();
        };
    }
    
    private void LoadData()
    {
        try
        {
            using var context = DatabaseService.GetContext();
            var controller = new EmployeeController(context);
            var employees = controller.GetActive();
            System.Diagnostics.Debug.WriteLine($"Загружено сотрудников: {employees.Count}");
            
            _employees.Clear();
            foreach (var emp in employees)
            {
                _employees.Add(emp);
            }
            
            System.Diagnostics.Debug.WriteLine($"ObservableCollection содержит: {_employees.Count} элементов");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            ShowError($"Ошибка загрузки данных: {ex.Message}");
        }
    }
    
    private void OnSearchTextChanged(object? sender, TextChangedEventArgs e)
    {
        var query = SearchBox.Text?.Trim();
        if (string.IsNullOrEmpty(query))
        {
            LoadData();
        }
        else
        {
            using var context = DatabaseService.GetContext();
            var controller = new EmployeeController(context);
            var results = controller.Search(query);
            _employees.Clear();
            foreach (var emp in results)
            {
                _employees.Add(emp);
            }
        }
    }
    
    private void OnSearch(object? sender, RoutedEventArgs e)
    {
        OnSearchTextChanged(sender, null!);
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
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
