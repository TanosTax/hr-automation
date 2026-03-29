using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class WorkScheduleController
{
    private readonly AppDbContext _context;
    
    public WorkScheduleController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<WorkSchedule> GetByEmployee(int employeeId, DateTime startDate, DateTime endDate)
    {
        return _context.WorkSchedules
            .Where(w => w.EmployeeId == employeeId && 
                       w.Date >= startDate && 
                       w.Date <= endDate)
            .OrderBy(w => w.Date)
            .ToList();
    }
    
    public List<WorkSchedule> GetByDate(DateTime date)
    {
        return _context.WorkSchedules
            .Include(w => w.Employee)
            .Where(w => w.Date.Date == date.Date)
            .ToList();
    }
    
    public void Add(WorkSchedule schedule)
    {
        _context.WorkSchedules.Add(schedule);
        _context.SaveChanges();
    }
    
    public void Update(WorkSchedule schedule)
    {
        _context.WorkSchedules.Update(schedule);
        _context.SaveChanges();
    }
    
    public void MarkAbsent(int employeeId, DateTime date, string reason)
    {
        var schedule = _context.WorkSchedules
            .FirstOrDefault(w => w.EmployeeId == employeeId && w.Date.Date == date.Date);
        
        if (schedule == null)
        {
            schedule = new WorkSchedule
            {
                EmployeeId = employeeId,
                Date = date,
                IsAbsent = true,
                AbsenceReason = reason
            };
            _context.WorkSchedules.Add(schedule);
        }
        else
        {
            schedule.IsAbsent = true;
            schedule.AbsenceReason = reason;
        }
        
        _context.SaveChanges();
    }
}
