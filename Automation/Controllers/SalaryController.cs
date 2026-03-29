using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class SalaryController
{
    private readonly AppDbContext _context;
    
    public SalaryController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Salary> GetAll()
    {
        return _context.Salaries
            .Include(s => s.Employee)
            .ThenInclude(e => e.Department)
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .ToList();
    }
    
    public List<Salary> GetByEmployee(int employeeId)
    {
        return _context.Salaries
            .Where(s => s.EmployeeId == employeeId)
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .ToList();
    }
    
    public List<Salary> GetByPeriod(int year, int month)
    {
        return _context.Salaries
            .Include(s => s.Employee)
            .ThenInclude(e => e.Department)
            .Where(s => s.Year == year && s.Month == month)
            .ToList();
    }
    
    public void Add(Salary salary)
    {
        salary.Total = salary.BaseSalary + salary.Bonus - salary.Deductions;
        _context.Salaries.Add(salary);
        _context.SaveChanges();
    }
    
    public void Update(Salary salary)
    {
        salary.Total = salary.BaseSalary + salary.Bonus - salary.Deductions;
        _context.Salaries.Update(salary);
        _context.SaveChanges();
    }
    
    public void MarkAsPaid(int id)
    {
        var salary = _context.Salaries.Find(id);
        if (salary != null)
        {
            salary.IsPaid = true;
            salary.PaymentDate = DateTime.UtcNow;
            _context.SaveChanges();
        }
    }
}
