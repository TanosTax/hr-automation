using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Automation.Controllers;
using Automation.Models;
using Automation.Services;

namespace Automation.Views;

public partial class ReportView : UserControl
{
    public ReportView()
    {
        InitializeComponent();
        
        this.Loaded += (s, e) =>
        {
            LoadStatistics();
        };
    }
    
    private void LoadStatistics()
    {
        using var context = DatabaseService.GetContext();
        
        // Общая статистика
        var employeeController = new EmployeeController(context);
        var departmentController = new DepartmentController(context);
        var vacationController = new VacationController(context);
        var salaryController = new SalaryController(context);
        
        var totalEmployees = employeeController.GetAll().Count;
        var totalDepartments = departmentController.GetAll().Count;
        var pendingVacations = vacationController.GetPending().Count;
        var unpaidSalaries = salaryController.GetAll().Count(s => !s.IsPaid);
        
        TxtTotalEmployees.Text = totalEmployees.ToString();
        TxtTotalDepartments.Text = totalDepartments.ToString();
        TxtPendingVacations.Text = pendingVacations.ToString();
        TxtUnpaidSalaries.Text = unpaidSalaries.ToString();
        
        // Статистика по отделам
        var departments = departmentController.GetAll();
        var departmentStats = new List<DepartmentStatDto>();
        
        foreach (var dept in departments)
        {
            var employees = employeeController.GetByDepartment(dept.Id);
            var avgSalary = employees.Any() ? employees.Average(e => e.Position.BaseSalary) : 0;
            var totalSalary = employees.Sum(e => e.Position.BaseSalary);
            
            departmentStats.Add(new DepartmentStatDto
            {
                DepartmentName = dept.Name,
                EmployeeCount = employees.Count,
                AverageSalary = avgSalary,
                TotalSalary = totalSalary
            });
        }
        
        DepartmentStatsGrid.ItemsSource = departmentStats;
        
        // Ближайшие дни рождения - показываем всех сотрудников, отсортированных по дате следующего ДР
        var allEmployees = employeeController.GetAll();
        var today = DateTime.Now;
        var upcomingBirthdays = allEmployees
            .Select(e => new
            {
                Employee = e,
                NextBirthday = GetNextBirthday(e.BirthDate, today),
                DaysUntil = (GetNextBirthday(e.BirthDate, today) - today).Days
            })
            .OrderBy(x => x.DaysUntil)
            .Take(10) // Показываем 10 ближайших
            .Select(x => x.Employee)
            .ToList();
        
        BirthdaysGrid.ItemsSource = upcomingBirthdays;
        
        // Отпуска - показываем все одобренные отпуска
        var activeVacations = vacationController.GetAll()
            .Where(v => v.Status == VacationStatus.Approved)
            .OrderByDescending(v => v.StartDate)
            .Take(10)
            .ToList();
        
        ActiveVacationsGrid.ItemsSource = activeVacations;
    }
    
    private DateTime GetNextBirthday(DateTime birthDate, DateTime today)
    {
        var nextBirthday = new DateTime(today.Year, birthDate.Month, birthDate.Day);
        if (nextBirthday < today)
            nextBirthday = nextBirthday.AddYears(1);
        return nextBirthday;
    }
}

public class DepartmentStatDto
{
    public string DepartmentName { get; set; } = "";
    public int EmployeeCount { get; set; }
    public decimal AverageSalary { get; set; }
    public decimal TotalSalary { get; set; }
}
