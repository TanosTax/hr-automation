using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class DocumentController
{
    private readonly AppDbContext _context;
    
    public DocumentController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Document> GetByEmployee(int employeeId)
    {
        return _context.Documents
            .Where(d => d.EmployeeId == employeeId)
            .OrderBy(d => d.Type)
            .ToList();
    }
    
    public void Add(Document document)
    {
        _context.Documents.Add(document);
        _context.SaveChanges();
    }
    
    public void Update(Document document)
    {
        _context.Documents.Update(document);
        _context.SaveChanges();
    }
    
    public void Delete(int id)
    {
        var document = _context.Documents.Find(id);
        if (document != null)
        {
            _context.Documents.Remove(document);
            _context.SaveChanges();
        }
    }
}
