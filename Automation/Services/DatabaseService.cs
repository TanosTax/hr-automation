using System;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Automation.Data;

namespace Automation.Services;

public class DatabaseService
{
    private static AppDbContext? _context;
    
    public static AppDbContext GetContext()
    {
        if (_context == null)
        {
            var connectionString = GetConnectionString();
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql(connectionString);
            
            _context = new AppDbContext(optionsBuilder.Options);
        }
        
        return _context;
    }
    
    private static string GetConnectionString()
    {
        try
        {
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (File.Exists(appSettingsPath))
            {
                var json = File.ReadAllText(appSettingsPath);
                var config = JsonDocument.Parse(json);
                var connectionString = config.RootElement
                    .GetProperty("ConnectionStrings")
                    .GetProperty("DefaultConnection")
                    .GetString();
                
                if (!string.IsNullOrEmpty(connectionString))
                    return connectionString;
            }
        }
        catch
        {
            // Fallback to default
        }
        
        return "Host=localhost;Port=5432;Database=hr_automation;Username=postgres;Password=0206";
    }
    
    public static void InitializeDatabase()
    {
        var context = GetContext();
        context.Database.Migrate();
    }
}
