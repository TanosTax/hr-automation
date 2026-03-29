using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? MiddleName { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [MaxLength(20)]
    public string? PassportSeries { get; set; }
    
    [MaxLength(20)]
    public string? PassportNumber { get; set; }
    
    [MaxLength(200)]
    public string? Address { get; set; }
    
    [MaxLength(20)]
    public string? Phone { get; set; }
    
    [MaxLength(100)]
    public string? Email { get; set; }
    
    [Required]
    public DateTime HireDate { get; set; }
    
    public DateTime? FireDate { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    [MaxLength(500)]
    public string? PhotoPath { get; set; }
    
    // Связи
    public int DepartmentId { get; set; }
    [ForeignKey(nameof(DepartmentId))]
    public Department Department { get; set; } = null!;
    
    public int PositionId { get; set; }
    [ForeignKey(nameof(PositionId))]
    public Position Position { get; set; } = null!;
    
    // Навигационные свойства
    public ICollection<Vacation> Vacations { get; set; } = new List<Vacation>();
    public ICollection<Salary> Salaries { get; set; } = new List<Salary>();
    public ICollection<WorkSchedule> WorkSchedules { get; set; } = new List<WorkSchedule>();
    public ICollection<Document> Documents { get; set; } = new List<Document>();
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    
    [NotMapped]
    public string FullName => $"{LastName} {FirstName} {MiddleName}".Trim();
}
