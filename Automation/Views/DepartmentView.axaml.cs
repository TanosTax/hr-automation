using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Services;

namespace Automation.Views;

public partial class DepartmentView : UserControl
{
    private readonly DepartmentController _controller;
    
    public DepartmentView()
    {
        InitializeComponent();
        _controller = new DepartmentController(DatabaseService.GetContext());
        LoadData();
    }
    
    private void LoadData()
    {
        try
        {
            var departments = _controller.GetAll();
            DepartmentsGrid.ItemsSource = departments;
        }
        catch (Exception ex)
        {
            ShowError($"Ошибка загрузки: {ex.Message}");
        }
    }
    
    private void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new DepartmentFormWindow();
        window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnEdit(object? sender, RoutedEventArgs e)
    {
        var selected = DepartmentsGrid.SelectedItem as Models.Department;
        if (selected != null)
        {
            var window = new DepartmentFormWindow(selected.Id);
            window.ShowDialog((Window)this.VisualRoot!);
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
            
            _controller.Delete(selected.Id);
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
