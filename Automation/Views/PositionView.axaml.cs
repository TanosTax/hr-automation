using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class PositionView : UserControl
{
    private ObservableCollection<Position> _positions = new();
    
    public PositionView()
    {
        InitializeComponent();
        
        PositionsGrid.ItemsSource = _positions;
        
        this.Loaded += (s, e) =>
        {
            LoadData();
        };
    }
    
    private void LoadData()
    {
        using var context = DatabaseService.GetContext();
        var controller = new PositionController(context);
        var positions = controller.GetAll();
        _positions.Clear();
        foreach (var pos in positions)
        {
            _positions.Add(pos);
        }
    }
    
    private async void OnAdd(object? sender, RoutedEventArgs e)
    {
        var window = new PositionFormWindow();
        await window.ShowDialog((Window)this.VisualRoot!);
        LoadData();
    }
    
    private async void OnEdit(object? sender, RoutedEventArgs e)
    {
        var selected = PositionsGrid.SelectedItem as Position;
        if (selected != null)
        {
            var window = new PositionFormWindow(selected.Id);
            await window.ShowDialog((Window)this.VisualRoot!);
            LoadData();
        }
    }
    
    private void OnDelete(object? sender, RoutedEventArgs e)
    {
        var selected = PositionsGrid.SelectedItem as Position;
        if (selected != null)
        {
            using var context = DatabaseService.GetContext();
            var controller = new PositionController(context);
            controller.Delete(selected.Id);
            LoadData();
        }
    }
}
