using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Automation.Migrations
{
    /// <inheritdoc />
    public partial class AddFullSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Address", "BirthDate", "DepartmentId", "Email", "FireDate", "FirstName", "HireDate", "IsActive", "LastName", "MiddleName", "PassportNumber", "PassportSeries", "Phone", "PhotoPath", "PositionId" },
                values: new object[,]
                {
                    { 1, "г. Москва, ул. Ленина, д. 10", new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, "petrov@company.ru", null, "Иван", new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Петров", "Сергеевич", null, null, "+7 (999) 123-45-67", null, 1 },
                    { 2, "г. Москва, ул. Пушкина, д. 5", new DateTime(1985, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), 2, "ivanova@company.ru", null, "Мария", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), true, "Иванова", "Петровна", null, null, "+7 (999) 234-56-78", null, 2 },
                    { 3, "г. Москва, ул. Гагарина, д. 20", new DateTime(1992, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 3, "smirnov@company.ru", null, "Алексей", new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Смирнов", "Дмитриевич", null, null, "+7 (999) 345-67-89", null, 3 },
                    { 4, "г. Москва, ул. Мира, д. 15", new DateTime(1990, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), 4, "kozlova@company.ru", null, "Елена", new DateTime(2021, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), true, "Козлова", "Александровна", null, null, "+7 (999) 456-78-90", null, 4 },
                    { 5, "г. Москва, ул. Советская, д. 8", new DateTime(1988, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, "novikov@company.ru", null, "Дмитрий", new DateTime(2022, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Новиков", "Иванович", null, null, "+7 (999) 567-89-01", null, 5 }
                });

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Requirements",
                value: "Высшее образование, опыт руководства от 10 лет");

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Requirements",
                value: "Высшее экономическое образование, опыт от 5 лет");

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Requirements",
                value: "Высшее техническое образование, знание C#/.NET");

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Requirements",
                value: "Опыт продаж от 2 лет, коммуникабельность");

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Requirements",
                value: "Среднее специальное образование, знание 1С");

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "ContractNumber", "EmployeeId", "EndDate", "IsActive", "Notes", "Salary", "SignDate", "Type" },
                values: new object[,]
                {
                    { 1, "К-2020-001", 1, null, true, "Бессрочный трудовой договор", 150000m, new DateTime(2020, 1, 10, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 2, "К-2020-002", 2, null, true, "Бессрочный трудовой договор", 80000m, new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 3, "К-2021-003", 3, new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, "Срочный трудовой договор на 3 года", 90000m, new DateTime(2021, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 4, "К-2021-004", 4, null, true, "Бессрочный трудовой договор", 60000m, new DateTime(2021, 9, 20, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 5, "К-2022-005", 5, null, true, "Бессрочный трудовой договор", 50000m, new DateTime(2022, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 0 }
                });

            migrationBuilder.InsertData(
                table: "Documents",
                columns: new[] { "Id", "EmployeeId", "FilePath", "IssueDate", "IssuedBy", "Notes", "Number", "Series", "Type" },
                values: new object[,]
                {
                    { 1, 1, null, new DateTime(2010, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), "ОВД Центрального района г. Москвы", null, "123456", "4510", 0 },
                    { 2, 2, null, new DateTime(2005, 8, 22, 0, 0, 0, 0, DateTimeKind.Utc), "ОВД Северного района г. Москвы", null, "234567", "4511", 0 },
                    { 3, 3, null, new DateTime(2014, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "Московский Государственный Университет", null, "МГУ 2014-12345", null, 3 },
                    { 4, 4, null, new DateTime(2010, 11, 5, 0, 0, 0, 0, DateTimeKind.Utc), "ОВД Южного района г. Москвы", null, "345678", "4512", 0 },
                    { 5, 5, null, new DateTime(2020, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Учебный центр 1С", "Сертификат 1С:Бухгалтерия", "CERT-2020-5678", null, 5 }
                });

            migrationBuilder.InsertData(
                table: "Salaries",
                columns: new[] { "Id", "BaseSalary", "Bonus", "Deductions", "EmployeeId", "IsPaid", "Month", "Notes", "PaymentDate", "Total", "Year" },
                values: new object[,]
                {
                    { 1, 150000m, 30000m, 23400m, 1, true, 3, "Премия за квартал", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2026 },
                    { 2, 80000m, 10000m, 11700m, 2, true, 3, "Стандартная выплата", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2026 },
                    { 3, 90000m, 15000m, 13650m, 3, false, 3, "Премия за проект", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2026 },
                    { 4, 60000m, 20000m, 10400m, 4, false, 3, "Премия за продажи", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2026 },
                    { 5, 50000m, 5000m, 7150m, 5, true, 3, "Стандартная выплата", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m, 2026 }
                });

            migrationBuilder.InsertData(
                table: "Vacations",
                columns: new[] { "Id", "Comment", "EmployeeId", "EndDate", "Reason", "StartDate", "Status", "Type" },
                values: new object[,]
                {
                    { 1, null, 1, new DateTime(2026, 7, 14, 0, 0, 0, 0, DateTimeKind.Utc), "Ежегодный оплачиваемый отпуск", new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 0 },
                    { 2, null, 2, new DateTime(2026, 8, 21, 0, 0, 0, 0, DateTimeKind.Utc), "Ежегодный оплачиваемый отпуск", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, 0 },
                    { 3, null, 3, new DateTime(2026, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Больничный лист", new DateTime(2026, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 4, null, 4, new DateTime(2026, 6, 24, 0, 0, 0, 0, DateTimeKind.Utc), "Ежегодный оплачиваемый отпуск", new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Utc), 0, 0 }
                });

            migrationBuilder.InsertData(
                table: "WorkSchedules",
                columns: new[] { "Id", "AbsenceReason", "CheckIn", "CheckOut", "Date", "EmployeeId", "IsAbsent", "Notes" },
                values: new object[,]
                {
                    { 1, null, new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false, null },
                    { 2, null, new TimeSpan(0, 9, 0, 0, 0), new TimeSpan(0, 18, 0, 0, 0), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, null },
                    { 3, null, new TimeSpan(0, 10, 0, 0, 0), new TimeSpan(0, 19, 0, 0, 0), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, false, null },
                    { 4, null, new TimeSpan(0, 9, 30, 0, 0), new TimeSpan(0, 18, 30, 0, 0), new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, null },
                    { 5, "Больничный", null, null, new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, true, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Contracts",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Documents",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Salaries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Salaries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Salaries",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Salaries",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Salaries",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vacations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vacations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vacations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vacations",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WorkSchedules",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Requirements",
                value: null);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 2,
                column: "Requirements",
                value: null);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 3,
                column: "Requirements",
                value: null);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 4,
                column: "Requirements",
                value: null);

            migrationBuilder.UpdateData(
                table: "Positions",
                keyColumn: "Id",
                keyValue: 5,
                column: "Requirements",
                value: null);
        }
    }
}
