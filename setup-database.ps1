# PowerShell script for database setup and application launch
# HR Automation System

Write-Host "=== HR Automation System - Setup ===" -ForegroundColor Cyan
Write-Host ""

# PostgreSQL connection parameters
$pgHost = "localhost"
$pgPort = "5432"
$pgUser = "postgres"
$pgPassword = "0206"
$dbName = "hr_automation"

# Find psql.exe
Write-Host "Looking for PostgreSQL..." -ForegroundColor Yellow
$psqlPath = Get-ChildItem "C:\Program Files\PostgreSQL" -Recurse -Filter "psql.exe" -ErrorAction SilentlyContinue | Select-Object -First 1 -ExpandProperty FullName

if (-not $psqlPath) {
    Write-Host "[ERROR] PostgreSQL not found" -ForegroundColor Red
    Write-Host "Install PostgreSQL from https://www.postgresql.org/download/" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host "[OK] PostgreSQL found at: $psqlPath" -ForegroundColor Green

# Check .NET SDK
Write-Host "Checking .NET SDK..." -ForegroundColor Yellow
try {
    $dotnetVersion = & dotnet --version 2>&1
    if ($LASTEXITCODE -ne 0) {
        throw ".NET SDK not found"
    }
    Write-Host "[OK] .NET SDK found (version $dotnetVersion)" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] .NET SDK is not installed" -ForegroundColor Red
    Write-Host "Install .NET 8 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Create database if not exists
Write-Host ""
Write-Host "Creating database..." -ForegroundColor Yellow
$env:PGPASSWORD = $pgPassword
$checkDb = & $psqlPath -h $pgHost -p $pgPort -U $pgUser -d postgres -tAc "SELECT 1 FROM pg_database WHERE datname='$dbName'" 2>&1

if ($checkDb -eq "1") {
    Write-Host "[OK] Database '$dbName' already exists" -ForegroundColor Green
} else {
    try {
        & $psqlPath -h $pgHost -p $pgPort -U $pgUser -d postgres -c "CREATE DATABASE $dbName;" 2>&1 | Out-Null
        if ($LASTEXITCODE -eq 0) {
            Write-Host "[OK] Database '$dbName' created" -ForegroundColor Green
        } else {
            throw "Database creation error"
        }
    } catch {
        Write-Host "[ERROR] Failed to create database" -ForegroundColor Red
        Read-Host "Press Enter to exit"
        exit 1
    }
}

# Apply migrations
Write-Host ""
Write-Host "Applying migrations..." -ForegroundColor Yellow
$originalPath = Get-Location
Set-Location -Path "Automation"
try {
    $migrationOutput = & dotnet ef database update 2>&1
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[OK] Migrations applied successfully" -ForegroundColor Green
    } else {
        Write-Host "Migration output:" -ForegroundColor Yellow
        Write-Host $migrationOutput
        throw "Migration error"
    }
} catch {
    Write-Host "[ERROR] Failed to apply migrations" -ForegroundColor Red
    Write-Host "Try manually: cd Automation && dotnet ef database update" -ForegroundColor Yellow
    Set-Location -Path $originalPath
    Read-Host "Press Enter to exit"
    exit 1
}

# Build application
Write-Host ""
Write-Host "Building application..." -ForegroundColor Yellow
try {
    & dotnet build -c Release 2>&1 | Out-Null
    if ($LASTEXITCODE -eq 0) {
        Write-Host "[OK] Application built" -ForegroundColor Green
    } else {
        throw "Build error"
    }
} catch {
    Write-Host "[ERROR] Failed to build application" -ForegroundColor Red
    Set-Location -Path $originalPath
    Read-Host "Press Enter to exit"
    exit 1
}

# Launch application
Write-Host ""
Write-Host "=== Launching Application ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Login credentials:" -ForegroundColor Yellow
Write-Host "  Administrator: admin / admin123" -ForegroundColor White
Write-Host "  HR Manager:    hr / hr123" -ForegroundColor White
Write-Host ""

& dotnet run -c Release
Set-Location -Path $originalPath
