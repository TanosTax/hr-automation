using System;
using Microsoft.EntityFrameworkCore;
using Automation.Models;

namespace Automation.Data;

public class AppDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Vacation> Vacations { get; set; }
    public DbSet<Contract> Contracts { get; set; }
    public DbSet<Salary> Salaries { get; set; }
    public DbSet<WorkSchedule> WorkSchedules { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<User> Users { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Индексы для оптимизации поиска
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.LastName);
        
        modelBuilder.Entity<Employee>()
            .HasIndex(e => e.IsActive);
        
        modelBuilder.Entity<Salary>()
            .HasIndex(s => new { s.Year, s.Month });
        
        modelBuilder.Entity<WorkSchedule>()
            .HasIndex(w => w.Date);
        
        // Начальные данные
        SeedData(modelBuilder);
    }
    
    private void SeedData(ModelBuilder modelBuilder)
    {
        // Отделы
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Администрация", Description = "Руководство компании" },
            new Department { Id = 2, Name = "Бухгалтерия", Description = "Финансовый отдел" },
            new Department { Id = 3, Name = "IT отдел", Description = "Информационные технологии" },
            new Department { Id = 4, Name = "Отдел продаж", Description = "Продажи и маркетинг" }
        );
        
        // Должности
        modelBuilder.Entity<Position>().HasData(
            new Position { Id = 1, Name = "Директор", BaseSalary = 150000, Requirements = "Высшее образование, опыт руководства от 10 лет" },
            new Position { Id = 2, Name = "Главный бухгалтер", BaseSalary = 80000, Requirements = "Высшее экономическое образование, опыт от 5 лет" },
            new Position { Id = 3, Name = "Программист", BaseSalary = 90000, Requirements = "Высшее техническое образование, знание C#/.NET" },
            new Position { Id = 4, Name = "Менеджер по продажам", BaseSalary = 60000, Requirements = "Опыт продаж от 2 лет, коммуникабельность" },
            new Position { Id = 5, Name = "Бухгалтер", BaseSalary = 50000, Requirements = "Среднее специальное образование, знание 1С" }
        );
        
        // Пользователи
        modelBuilder.Entity<User>().HasData(
            new User 
            { 
                Id = 1, 
                Username = "admin", 
                Password = "admin123", 
                Role = UserRole.Admin,
                FullName = "Администратор Системы",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new User 
            { 
                Id = 2, 
                Username = "hr", 
                Password = "hr123", 
                Role = UserRole.HRManager,
                FullName = "HR Менеджер",
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
        
        // Сотрудники
        modelBuilder.Entity<Employee>().HasData(
            new Employee 
            { 
                Id = 1, 
                FirstName = "Иван", 
                LastName = "Петров", 
                MiddleName = "Сергеевич",
                BirthDate = new DateTime(1980, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                Phone = "+7 (999) 123-45-67",
                Email = "petrov@company.ru",
                Address = "г. Москва, ул. Ленина, д. 10",
                DepartmentId = 1,
                PositionId = 1,
                HireDate = new DateTime(2020, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Employee 
            { 
                Id = 2, 
                FirstName = "Мария", 
                LastName = "Иванова", 
                MiddleName = "Петровна",
                BirthDate = new DateTime(1985, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                Phone = "+7 (999) 234-56-78",
                Email = "ivanova@company.ru",
                Address = "г. Москва, ул. Пушкина, д. 5",
                DepartmentId = 2,
                PositionId = 2,
                HireDate = new DateTime(2020, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Employee 
            { 
                Id = 3, 
                FirstName = "Алексей", 
                LastName = "Смирнов", 
                MiddleName = "Дмитриевич",
                BirthDate = new DateTime(1992, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                Phone = "+7 (999) 345-67-89",
                Email = "smirnov@company.ru",
                Address = "г. Москва, ул. Гагарина, д. 20",
                DepartmentId = 3,
                PositionId = 3,
                HireDate = new DateTime(2021, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Employee 
            { 
                Id = 4, 
                FirstName = "Елена", 
                LastName = "Козлова", 
                MiddleName = "Александровна",
                BirthDate = new DateTime(1990, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                Phone = "+7 (999) 456-78-90",
                Email = "kozlova@company.ru",
                Address = "г. Москва, ул. Мира, д. 15",
                DepartmentId = 4,
                PositionId = 4,
                HireDate = new DateTime(2021, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            },
            new Employee 
            { 
                Id = 5, 
                FirstName = "Дмитрий", 
                LastName = "Новиков", 
                MiddleName = "Иванович",
                BirthDate = new DateTime(1988, 7, 18, 0, 0, 0, DateTimeKind.Utc),
                Phone = "+7 (999) 567-89-01",
                Email = "novikov@company.ru",
                Address = "г. Москва, ул. Советская, д. 8",
                DepartmentId = 2,
                PositionId = 5,
                HireDate = new DateTime(2022, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true
            }
        );
        
        // Контракты
        modelBuilder.Entity<Contract>().HasData(
            new Contract 
            { 
                Id = 1, 
                EmployeeId = 1, 
                ContractNumber = "К-2020-001",
                SignDate = new DateTime(2020, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null,
                Type = ContractType.Permanent,
                Salary = 150000,
                Notes = "Бессрочный трудовой договор",
                IsActive = true
            },
            new Contract 
            { 
                Id = 2, 
                EmployeeId = 2, 
                ContractNumber = "К-2020-002",
                SignDate = new DateTime(2020, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null,
                Type = ContractType.Permanent,
                Salary = 80000,
                Notes = "Бессрочный трудовой договор",
                IsActive = true
            },
            new Contract 
            { 
                Id = 3, 
                EmployeeId = 3, 
                ContractNumber = "К-2021-003",
                SignDate = new DateTime(2021, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                Type = ContractType.Fixed,
                Salary = 90000,
                Notes = "Срочный трудовой договор на 3 года",
                IsActive = true
            },
            new Contract 
            { 
                Id = 4, 
                EmployeeId = 4, 
                ContractNumber = "К-2021-004",
                SignDate = new DateTime(2021, 9, 20, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null,
                Type = ContractType.Permanent,
                Salary = 60000,
                Notes = "Бессрочный трудовой договор",
                IsActive = true
            },
            new Contract 
            { 
                Id = 5, 
                EmployeeId = 5, 
                ContractNumber = "К-2022-005",
                SignDate = new DateTime(2022, 2, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = null,
                Type = ContractType.Permanent,
                Salary = 50000,
                Notes = "Бессрочный трудовой договор",
                IsActive = true
            }
        );
        
        // Отпуска
        modelBuilder.Entity<Vacation>().HasData(
            new Vacation 
            { 
                Id = 1, 
                EmployeeId = 1, 
                Type = VacationType.Vacation,
                StartDate = new DateTime(2026, 7, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 7, 14, 0, 0, 0, DateTimeKind.Utc),
                Status = VacationStatus.Approved,
                Reason = "Ежегодный оплачиваемый отпуск"
            },
            new Vacation 
            { 
                Id = 2, 
                EmployeeId = 2, 
                Type = VacationType.Vacation,
                StartDate = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 8, 21, 0, 0, 0, DateTimeKind.Utc),
                Status = VacationStatus.Pending,
                Reason = "Ежегодный оплачиваемый отпуск"
            },
            new Vacation 
            { 
                Id = 3, 
                EmployeeId = 3, 
                Type = VacationType.SickLeave,
                StartDate = new DateTime(2026, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 3, 20, 0, 0, 0, DateTimeKind.Utc),
                Status = VacationStatus.Approved,
                Reason = "Больничный лист"
            },
            new Vacation 
            { 
                Id = 4, 
                EmployeeId = 4, 
                Type = VacationType.Vacation,
                StartDate = new DateTime(2026, 6, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2026, 6, 24, 0, 0, 0, DateTimeKind.Utc),
                Status = VacationStatus.Pending,
                Reason = "Ежегодный оплачиваемый отпуск"
            }
        );
        
        // Зарплаты
        modelBuilder.Entity<Salary>().HasData(
            new Salary 
            { 
                Id = 1, 
                EmployeeId = 1, 
                Year = 2026, 
                Month = 3,
                BaseSalary = 150000,
                Bonus = 30000,
                Deductions = 23400,
                IsPaid = true,
                Notes = "Премия за квартал"
            },
            new Salary 
            { 
                Id = 2, 
                EmployeeId = 2, 
                Year = 2026, 
                Month = 3,
                BaseSalary = 80000,
                Bonus = 10000,
                Deductions = 11700,
                IsPaid = true,
                Notes = "Стандартная выплата"
            },
            new Salary 
            { 
                Id = 3, 
                EmployeeId = 3, 
                Year = 2026, 
                Month = 3,
                BaseSalary = 90000,
                Bonus = 15000,
                Deductions = 13650,
                IsPaid = false,
                Notes = "Премия за проект"
            },
            new Salary 
            { 
                Id = 4, 
                EmployeeId = 4, 
                Year = 2026, 
                Month = 3,
                BaseSalary = 60000,
                Bonus = 20000,
                Deductions = 10400,
                IsPaid = false,
                Notes = "Премия за продажи"
            },
            new Salary 
            { 
                Id = 5, 
                EmployeeId = 5, 
                Year = 2026, 
                Month = 3,
                BaseSalary = 50000,
                Bonus = 5000,
                Deductions = 7150,
                IsPaid = true,
                Notes = "Стандартная выплата"
            }
        );
        
        // Табель
        modelBuilder.Entity<WorkSchedule>().HasData(
            new WorkSchedule 
            { 
                Id = 1, 
                EmployeeId = 1, 
                Date = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckIn = new TimeSpan(9, 0, 0),
                CheckOut = new TimeSpan(18, 0, 0),
                IsAbsent = false
            },
            new WorkSchedule 
            { 
                Id = 2, 
                EmployeeId = 2, 
                Date = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckIn = new TimeSpan(9, 0, 0),
                CheckOut = new TimeSpan(18, 0, 0),
                IsAbsent = false
            },
            new WorkSchedule 
            { 
                Id = 3, 
                EmployeeId = 3, 
                Date = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckIn = new TimeSpan(10, 0, 0),
                CheckOut = new TimeSpan(19, 0, 0),
                IsAbsent = false
            },
            new WorkSchedule 
            { 
                Id = 4, 
                EmployeeId = 4, 
                Date = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckIn = new TimeSpan(9, 30, 0),
                CheckOut = new TimeSpan(18, 30, 0),
                IsAbsent = false
            },
            new WorkSchedule 
            { 
                Id = 5, 
                EmployeeId = 5, 
                Date = new DateTime(2026, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                CheckIn = null,
                CheckOut = null,
                IsAbsent = true,
                AbsenceReason = "Больничный"
            }
        );
        
        // Документы
        modelBuilder.Entity<Document>().HasData(
            new Document 
            { 
                Id = 1, 
                EmployeeId = 1, 
                Type = DocumentType.Passport,
                Number = "123456",
                Series = "4510",
                IssueDate = new DateTime(2010, 5, 15, 0, 0, 0, DateTimeKind.Utc),
                IssuedBy = "ОВД Центрального района г. Москвы"
            },
            new Document 
            { 
                Id = 2, 
                EmployeeId = 2, 
                Type = DocumentType.Passport,
                Number = "234567",
                Series = "4511",
                IssueDate = new DateTime(2005, 8, 22, 0, 0, 0, DateTimeKind.Utc),
                IssuedBy = "ОВД Северного района г. Москвы"
            },
            new Document 
            { 
                Id = 3, 
                EmployeeId = 3, 
                Type = DocumentType.EducationDiploma,
                Number = "МГУ 2014-12345",
                IssueDate = new DateTime(2014, 6, 30, 0, 0, 0, DateTimeKind.Utc),
                IssuedBy = "Московский Государственный Университет"
            },
            new Document 
            { 
                Id = 4, 
                EmployeeId = 4, 
                Type = DocumentType.Passport,
                Number = "345678",
                Series = "4512",
                IssueDate = new DateTime(2010, 11, 5, 0, 0, 0, DateTimeKind.Utc),
                IssuedBy = "ОВД Южного района г. Москвы"
            },
            new Document 
            { 
                Id = 5, 
                EmployeeId = 5, 
                Type = DocumentType.Other,
                Number = "CERT-2020-5678",
                IssueDate = new DateTime(2020, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                IssuedBy = "Учебный центр 1С",
                Notes = "Сертификат 1С:Бухгалтерия"
            }
        );
    }
}
