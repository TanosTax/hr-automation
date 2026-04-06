using Automation.Models;

namespace Automation.Services;

public static class AuthService
{
    public static User? CurrentUser { get; private set; }
    
    public static bool IsAdmin => CurrentUser?.Role == UserRole.Admin;
    public static bool IsHRManager => CurrentUser?.Role == UserRole.HRManager;
    
    public static void Login(User user)
    {
        CurrentUser = user;
    }
    
    public static void Logout()
    {
        CurrentUser = null;
    }
    
    public static bool HasAccess(UserRole requiredRole)
    {
        if (CurrentUser == null) return false;
        
        // Admin имеет доступ ко всему
        if (CurrentUser.Role == UserRole.Admin) return true;
        
        // Проверяем конкретную роль
        return CurrentUser.Role == requiredRole;
    }
}
