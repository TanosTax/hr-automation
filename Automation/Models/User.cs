using System;

namespace Automation.Models;

public enum UserRole
{
    Admin,
    HRManager
}

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = ""; // В реальном приложении хешировать!
    public UserRole Role { get; set; }
    public string FullName { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
