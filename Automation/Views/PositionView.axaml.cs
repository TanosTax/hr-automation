using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class PositionView : UserControl
{
    private readonly PositionController _controller;
    
    public PositionView()
    {
        InitializeComponent();
        _controller = new PositionController(DatabaseService.GetContext());
        LoadData();
    }
    
    private void LoadData()
    {
        var positions = _controller.GetAll();
        PositionsGrid.ItemsSource = positions;
    }
    
    private void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new PositionFormWindow();
        window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private void OnEdit(object? sender, RoutedEventArgs e)
    {
        var selected = PositionsGrid.SelectedItem as Position;
        if (selected != null)
        {
            var window = new PositionFormWindow(selected.Id);
            window.ShowDialog((Window)this.VisualRoot!);
            LoadData();
        }
    }
    
    private void OnDelete(object? sender, RoutedEventArgs e)
    {
        var selected = PositionsGrid.SelectedItem as Position;
        if (selected != null)
        {
            _controller.Delete(selected.Id);
            LoadData();
        }
    }
}
