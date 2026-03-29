using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public enum VacationType
{
    Vacation,      // Отпуск
    SickLeave,     // Больничный
    Unpaid         // Без содержания
}

public enum VacationStatus
{
    Pending,       // Ожидает
    Approved,      // Одобрен
    Rejected,      // Отклонен
    Completed      // Завершен
}

public class Vacation
{
    [Key]
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
    
    [Required]
    public VacationType Type { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public VacationStatus Status { get; set; } = VacationStatus.Pending;
    
    [MaxLength(500)]
    public string? Reason { get; set; }
    
    [MaxLength(500)]
    public string? Comment { get; set; }
    
    [NotMapped]
    public int DaysCount => (EndDate - StartDate).Days + 1;
}
