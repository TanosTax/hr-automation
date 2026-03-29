using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class SalaryView : UserControl
{
    private readonly SalaryController _controller;
    
    public SalaryView()
    {
        InitializeComponent();
        _controller = new SalaryController(DatabaseService.GetContext());
        
        NumYear.Value = DateTime.Now.Year;
        NumMonth.Value = DateTime.Now.Month;
        
        LoadData();
    }
    
    private void LoadData()
    {
        var salaries = _controller.GetAll();
        SalariesGrid.ItemsSource = salaries;
    }
    
    private void OnFilter(object? sender, RoutedEventArgs e)
    {
        var year = (int)(NumYear.Value ?? DateTime.Now.Year);
        var month = (int)(NumMonth.Value ?? DateTime.Now.Month);
        
        var salaries = _controller.GetByPeriod(year, month);
        SalariesGrid.ItemsSource = salaries;
    }
    
    private void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new SalaryFormWindow();
        window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnMarkPaid(object? sender, RoutedEventArgs e)
    {
        var selected = SalariesGrid.SelectedItem as Salary;
        if (selected != null && !selected.IsPaid)
        {
            _controller.MarkAsPaid(selected.Id);
            LoadData();
        }
    }
}
