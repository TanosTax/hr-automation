using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Automation.Models;

public enum DocumentType
{
    Passport,          // Паспорт
    INN,              // ИНН
    SNILS,            // СНИЛС
    EducationDiploma, // Диплом
    MilitaryID,       // Военный билет
    Other             // Другое
}

public class Document
{
    [Key]
    public int Id { get; set; }
    
    public int EmployeeId { get; set; }
    [ForeignKey(nameof(EmployeeId))]
    public Employee Employee { get; set; } = null!;
    
    [Required]
    public DocumentType Type { get; set; }
    
    [Required, MaxLength(100)]
    public string Number { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string? Series { get; set; }
    
    [Required]
    public DateTime IssueDate { get; set; }
    
    [MaxLength(200)]
    public string? IssuedBy { get; set; }
    
    [MaxLength(500)]
    public string? FilePath { get; set; }
    
    [MaxLength(500)]
    public string? Notes { get; set; }
}
