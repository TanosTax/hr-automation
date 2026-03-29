using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class DepartmentView : UserControl
{
    private ObservableCollection<Department> _departments = new();
    
    public DepartmentView()
    {
        InitializeComponent();
        System.Diagnostics.Debug.WriteLine("DepartmentView: Инициализация...");
        
        DepartmentsGrid.ItemsSource = _departments;
        
        this.Loaded += (s, e) =>
        {
            System.Diagnostics.Debug.WriteLine("DepartmentView: Loaded event");
            LoadData();
        };
    }
    
    private void LoadData()
    {
        try
        {
            using var context = DatabaseService.GetContext();
            var controller = new DepartmentController(context);
            var departments = controller.GetAll();
            System.Diagnostics.Debug.WriteLine($"Загружено отделов: {departments.Count}");
            
            _departments.Clear();
            foreach (var dept in departments)
            {
                _departments.Add(dept);
            }
            
            System.Diagnostics.Debug.WriteLine($"ObservableCollection содержит: {_departments.Count} элементов");
            System.Diagnostics.Debug.WriteLine($"DataGrid ItemsSource установлен, элементов: {_departments.Count}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            ShowError($"Ошибка загрузки: {ex.Message}");
        }
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new DepartmentFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private async void OnEdit(object? sender, RoutedEventArgs e)
    {
        var selected = DepartmentsGrid.SelectedItem as Models.Department;
        if (selected != null)
        {
            var window = new DepartmentFormWindow(selected.Id);
            await window.ShowDialog((Window)this.VisualRoot!);
            LoadData();
        }
    }
    
    private async void OnDelete(object? sender, RoutedEventArgs e)
    {
        var selected = DepartmentsGrid.SelectedItem as Models.Department;
        if (selected != null)
        {
            if (selected.Employees.Count > 0)
            {
                await ShowMessage("Нельзя удалить отдел с сотрудниками!");
                return;
            }
            
            using var context = DatabaseService.GetContext();
            var controller = new DepartmentController(context);
            controller.Delete(selected.Id);
            LoadData();
        }
    }
    
    private void ShowError(string message)
    {
        // Простая обработка ошибок
    }
    
    private async System.Threading.Tasks.Task ShowMessage(string message)
    {
        var dialog = new Window
        {
            Title = "Сообщение",
            Width = 400,
            Height = 150,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Content = new StackPanel
            {
                Margin = new Avalonia.Thickness(20),
                Children =
                {
                    new TextBlock { Text = message, Margin = new Avalonia.Thickness(0, 0, 0, 20) },
                    new Button { Content = "OK", Width = 80, HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right }
                }
            }
        };
        
        var btn = ((StackPanel)dialog.Content).Children[1] as Button;
        btn!.Click += (s, e) => dialog.Close();
        
        await dialog.ShowDialog((Window)this.VisualRoot!);
    }
}
