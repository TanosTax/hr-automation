using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class SalaryView : UserControl
{
    private ObservableCollection<Salary> _salaries = new();
    
    public SalaryView()
    {
        InitializeComponent();
        
        SalariesGrid.ItemsSource = _salaries;
        
        NumYear.Value = DateTime.Now.Year;
        NumMonth.Value = DateTime.Now.Month;
        
        this.Loaded += (s, e) =>
        {
            LoadData();
        };
    }
    
    private void LoadData()
    {
        using var context = DatabaseService.GetContext();
        var controller = new SalaryController(context);
        var salaries = controller.GetAll();
        _salaries.Clear();
        foreach (var sal in salaries)
        {
            _salaries.Add(sal);
        }
    }
    
    private void OnFilter(object? sender, RoutedEventArgs e)
    {
        var year = (int)(NumYear.Value ?? DateTime.Now.Year);
        var month = (int)(NumMonth.Value ?? DateTime.Now.Month);
        
        using var context = DatabaseService.GetContext();
        var controller = new SalaryController(context);
        var salaries = controller.GetByPeriod(year, month);
        _salaries.Clear();
        foreach (var sal in salaries)
        {
            _salaries.Add(sal);
        }
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new SalaryFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnMarkPaid(object? sender, RoutedEventArgs e)
    {
        var selected = SalariesGrid.SelectedItem as Salary;
        if (selected != null && !selected.IsPaid)
        {
            using var context = DatabaseService.GetContext();
            var controller = new SalaryController(context);
            controller.MarkAsPaid(selected.Id);
            LoadData();
        }
    }
}
