using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class PositionFormWindow : Window
{
    private readonly PositionController _controller;
    private readonly int? _positionId;
    
    public PositionFormWindow(int? positionId = null)
    {
        InitializeComponent();
        _controller = new PositionController(DatabaseService.GetContext());
        _positionId = positionId;
        
        if (_positionId.HasValue)
        {
            LoadPosition(_positionId.Value);
            Title = "Редактирование должности";
        }
        else
        {
            Title = "Новая должность";
            NumSalary.Value = 50000;
        }
    }
    
    private void LoadPosition(int id)
    {
        var position = _controller.GetById(id);
        if (position != null)
        {
            TxtName.Text = position.Name;
            NumSalary.Value = position.BaseSalary;
            TxtRequirements.Text = position.Requirements;
        }
    }
    
    private void OnSave(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtName.Text))
            return;
        
        try
        {
            var position = _positionId.HasValue 
                ? _controller.GetById(_positionId.Value)!
                : new Position();
            
            position.Name = TxtName.Text;
            position.BaseSalary = NumSalary.Value ?? 0;
            position.Requirements = TxtRequirements.Text;
            
            if (_positionId.HasValue)
                _controller.Update(position);
            else
                _controller.Add(position);
            
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
