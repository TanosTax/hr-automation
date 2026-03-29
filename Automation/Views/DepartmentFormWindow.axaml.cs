using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class DepartmentFormWindow : Window
{
    private readonly DepartmentController _controller;
    private readonly int? _departmentId;
    
    public DepartmentFormWindow(int? departmentId = null)
    {
        InitializeComponent();
        _controller = new DepartmentController(DatabaseService.GetContext());
        _departmentId = departmentId;
        
        if (_departmentId.HasValue)
        {
            LoadDepartment(_departmentId.Value);
            Title = "Редактирование отдела";
        }
        else
        {
            Title = "Новый отдел";
        }
    }
    
    private void LoadDepartment(int id)
    {
        var department = _controller.GetById(id);
        if (department != null)
        {
            TxtName.Text = department.Name;
            TxtDescription.Text = department.Description;
        }
    }
    
    private void OnSave(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text))
            return;
        
        try
        {
            var department = _departmentId.HasValue 
                ? _controller.GetById(_departmentId.Value)!
                : new Department();
            
            department.Name = TxtName.Text;
            department.Description = TxtDescription.Text;
            
            if (_departmentId.HasValue)
                _controller.Update(department);
            else
                _controller.Add(department);
            
            Close();
        }
        catch (Exception)
        {
            // Обработка ошибок
        }
    }
    
    private void OnCancel(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}
