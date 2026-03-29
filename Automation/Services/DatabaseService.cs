using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Automation.Data;

namespace Automation.Services;

public class DatabaseService
{
    public static AppDbContext GetContext()
    {
        var connectionString = GetConnectionString();
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
    
    private static string GetConnectionString()
    {
        try
        {
            var appSettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            System.Diagnostics.Debug.WriteLine($"Ищем appsettings.json в: {appSettingsPath}");
            
            if (File.Exists(appSettingsPath))
            {
                var json = File.ReadAllText(appSettingsPath);
                System.Diagnostics.Debug.WriteLine($"Содержимое appsettings.json: {json}");
                
                var config = JsonDocument.Parse(json);
                var connectionString = config.RootElement
                    .GetProperty("ConnectionStrings")
                    .GetProperty("DefaultConnection")
                    .GetString();
                
                if (!string.IsNullOrEmpty(connectionString))
                {
                    System.Diagnostics.Debug.WriteLine($"Строка подключения: {connectionString}");
                    return connectionString;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("appsettings.json не найден!");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Ошибка чтения конфига: {ex.Message}");
        }
        
        var defaultConnection = "Host=localhost;Port=5432;Database=hr_automation;Username=postgres;Password=0206";
        System.Diagnostics.Debug.WriteLine($"Используем дефолтную строку: {defaultConnection}");
        return defaultConnection;
    }
    
    public static void InitializeDatabase()
    {
        try
        {
            var context = GetContext();
            System.Diagnostics.Debug.WriteLine("Подключение к БД...");
            context.Database.Migrate();
            System.Diagnostics.Debug.WriteLine("Миграции применены успешно");
            
            // Проверяем данные
            var deptCount = context.Departments.ToList().Count;
            var empCount = context.Employees.ToList().Count;
            System.Diagnostics.Debug.WriteLine($"В БД: {deptCount} отделов, {empCount} сотрудников");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"ОШИБКА инициализации БД: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
            throw;
        }
    }
}
