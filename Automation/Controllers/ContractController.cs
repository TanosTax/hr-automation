using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class ContractController
{
    private readonly AppDbContext _context;
    
    public ContractController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Contract> GetByEmployee(int employeeId)
    {
        return _context.Contracts
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.SignDate)
            .ToList();
    }
    
    public Contract? GetActive(int employeeId)
    {
        return _context.Contracts
            .FirstOrDefault(c => c.EmployeeId == employeeId && c.IsActive);
    }
    
    public void Add(Contract contract)
    {
        _context.Contracts.Add(contract);
        _context.SaveChanges();
    }
    
    public void Update(Contract contract)
    {
        _context.Contracts.Update(contract);
        _context.SaveChanges();
    }
    
    public void Terminate(int id)
    {
        var contract = _context.Contracts.Find(id);
        if (contract != null)
        {
            contract.IsActive = false;
            _context.SaveChanges();
        }
    }
}
