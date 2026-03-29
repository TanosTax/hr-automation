using System.Collections.Generic;
using System.Linq;
using Automation.Data;
using Automation.Models;

namespace Automation.Controllers;

public class PositionController
{
    private readonly AppDbContext _context;
    
    public PositionController(AppDbContext context)
    {
        _context = context;
    }
    
    public List<Position> GetAll()
    {
        return _context.Positions.OrderBy(p => p.Name).ToList();
    }
    
    public Position? GetById(int id)
    {
        return _context.Positions.Find(id);
    }
    
    public void Add(Position position)
    {
        _context.Positions.Add(position);
        _context.SaveChanges();
    }
    
    public void Update(Position position)
    {
        _context.Positions.Update(position);
        _context.SaveChanges();
    }
    
    public void Delete(int id)
    {
        var position = _context.Positions.Find(id);
        if (position != null)
        {
            _context.Positions.Remove(position);
            _context.SaveChanges();
        }
    }
}
