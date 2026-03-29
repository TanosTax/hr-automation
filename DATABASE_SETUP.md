# Настройка базы данных PostgreSQL

## Шаги для запуска:

1. **Установить PostgreSQL и pgAdmin4** (если еще не установлены)
   - Скачать с https://www.postgresql.org/download/

2. **Создать базу данных через pgAdmin4:**
   - Открыть pgAdmin4
   - Подключиться к серверу PostgreSQL
   - Правой кнопкой на Databases → Create → Database
   - Имя базы: `hr_automation`
   - Owner: `postgres`

3. **Настроить подключение:**
   - Открыть файл `appsettings.json`
   - Изменить строку подключения если нужно:
     ```json
     "DefaultConnection": "Host=localhost;Port=5432;Database=hr_automation;Username=postgres;Password=ВАШ_ПАРОЛЬ"
     ```

4. **Применить миграции:**
   ```bash
   cd Automation
   dotnet ef database update
   ```

5. **Проверить в pgAdmin4:**
   - Обновить список таблиц в базе `hr_automation`
   - Должны появиться таблицы: Employees, Departments, Positions, Vacations, Contracts, Salaries, WorkSchedules, Documents

## Команды для работы с миграциями:

- Создать новую миграцию: `dotnet ef migrations add ИмяМиграции`
- Применить миграции: `dotnet ef database update`
- Откатить миграцию: `dotnet ef database update ПредыдущаяМиграция`
- Удалить последнюю миграцию: `dotnet ef migrations remove`
