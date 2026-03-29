using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class VacationController
{
    private readonly AppDbContext _context;
    
    public VacationController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Vacation> GetAll()
    {
        return _context.Vacations
            .Include(v => v.Employee)
                .ThenInclude(e => e.Department)
            .OrderByDescending(v => v.StartDate)
            .ToList();
    }
    
    public List<Vacation> GetByEmployee(int employeeId)
    {
        return _context.Vacations
            .Where(v => v.EmployeeId == employeeId)
            .OrderByDescending(v => v.StartDate)
            .ToList();
    }
    
    public List<Vacation> GetPending()
    {
        return _context.Vacations
            .Include(v => v.Employee)
                .ThenInclude(e => e.Department)
            .Where(v => v.Status == VacationStatus.Pending)
            .ToList();
    }
    
    public void Add(Vacation vacation)
    {
        _context.Vacations.Add(vacation);
        _context.SaveChanges();
    }
    
    public void Update(Vacation vacation)
    {
        _context.Vacations.Update(vacation);
        _context.SaveChanges();
    }
    
    public void Approve(int id)
    {
        var vacation = _context.Vacations.Find(id);
        if (vacation != null)
        {
            vacation.Status = VacationStatus.Approved;
            _context.SaveChanges();
        }
    }
    
    public void Reject(int id, string comment)
    {
        var vacation = _context.Vacations.Find(id);
        if (vacation != null)
        {
            vacation.Status = VacationStatus.Rejected;
            vacation.Comment = comment;
            _context.SaveChanges();
        }
    }
}
