using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class EmployeeController
{
    private readonly AppDbContext _context;
    
    public EmployeeController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Employee> GetAll()
    {
        return _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .OrderBy(e => e.LastName)
            .ToList();
    }
    
    public List<Employee> GetActive()
    {
        return _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Where(e => e.IsActive)
            .OrderBy(e => e.LastName)
            .ToList();
    }
    
    public Employee? GetById(int id)
    {
        return _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.Vacations)
            .Include(e => e.Contracts)
            .Include(e => e.Documents)
            .FirstOrDefault(e => e.Id == id);
    }
    
    public List<Employee> Search(string query)
    {
        query = query.ToLower();
        return _context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .Where(e => e.LastName.ToLower().Contains(query) ||
                       e.FirstName.ToLower().Contains(query) ||
                       (e.MiddleName != null && e.MiddleName.ToLower().Contains(query)) ||
                       e.Email!.ToLower().Contains(query))
            .ToList();
    }
    
    public void Add(Employee employee)
    {
        _context.Employees.Add(employee);
        _context.SaveChanges();
    }
    
    public void Update(Employee employee)
    {
        _context.Employees.Update(employee);
        _context.SaveChanges();
    }
    
    public void Delete(int id)
    {
        var employee = _context.Employees.Find(id);
        if (employee != null)
        {
            employee.IsActive = false;
            employee.FireDate = DateTime.Now;
            _context.SaveChanges();
        }
    }
    
    public List<Employee> GetByDepartment(int departmentId)
    {
        return _context.Employees
            .Include(e => e.Position)
            .Where(e => e.DepartmentId == departmentId && e.IsActive)
            .ToList();
    }
}
