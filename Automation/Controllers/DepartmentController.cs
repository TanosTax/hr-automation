using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class DepartmentController
{
    private readonly AppDbContext _context;
    
    public DepartmentController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Department> GetAll()
    {
        return _context.Departments
            .Include(d => d.Employees)
            .OrderBy(d => d.Name)
            .ToList();
    }
    
    public Department? GetById(int id)
    {
        return _context.Departments
            .Include(d => d.Employees)
            .FirstOrDefault(d => d.Id == id);
    }
    
    public void Add(Department department)
    {
        _context.Departments.Add(department);
        _context.SaveChanges();
    }
    
    public void Update(Department department)
    {
        _context.Departments.Update(department);
        _context.SaveChanges();
    }
    
    public void Delete(int id)
    {
        var department = _context.Departments.Find(id);
        if (department != null && !department.Employees.Any())
        {
            _context.Departments.Remove(department);
            _context.SaveChanges();
        }
    }
}
