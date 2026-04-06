using System;
using Microsoft.EntityFrameworkCore;
using Automation.Models;

namespace Automation.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Vacation> Vacations { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<User> Users { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Индексы для оптимизации поиска
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.LastName);
        
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.IsActive);
        
        modelBuilder.Entity<Salary>()
            .HasIndex(s => new { s.Year, s.Month });
        
        modelBuilder.Entity<WorkSchedule>()
            .HasIndex(w => w.Date);
        
        // Начальные данные
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Отделы
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Администрация", Description = "Руководство компании" },
            new Department { Id = 2, Name = "Бухгалтерия", Description = "Финансовый отдел" },
            new Department { Id = 3, Name = "IT отдел", Description = "Информационные технологии" },
            new Department { Id = 4, Name = "Отдел продаж", Description = "Продажи и маркетинг" }
        );
        
        // Должности
        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Директор", BaseSalary = 150000 },
            new Position { Id = 2, Name = "Главный бухгалтер", BaseSalary = 80000 },
            new Position { Id = 3, Name = "Программист", BaseSalary = 90000 },
            new Position { Id = 4, Name = "Менеджер по продажам", BaseSalary = 60000 },
            new Position { Id = 5, Name = "Бухгалтер", BaseSalary = 50000 }
        );
        
        // Пользователи
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Password = "admin123", 
                Role = UserRole.Admin,
                FullName = "Администратор Системы",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new User 
            { 
                Id = 2, 
                Username = "hr", 
                Password = "hr123", 
                Role = UserRole.HRManager,
                FullName = "HR Менеджер",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
    }
}
