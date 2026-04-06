@echo off
chcp 65001 >nul
title HR Automation System - Запуск

echo.
echo ╔════════════════════════════════════════════════════════╗
echo ║     HR Automation System - Автоматический запуск      ║
echo ╚════════════════════════════════════════════════════════╝
echo.

REM Проверка PowerShell
where powershell >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ОШИБКА] PowerShell не найден
    pause
    exit /b 1
)

REM Запуск PowerShell скрипта
powershell -ExecutionPolicy Bypass -File "%~dp0setup-database.ps1"

if %ERRORLEVEL% NEQ 0 (
    echo.
    echo [ОШИБКА] Произошла ошибка при запуске
    pause
    exit /b 1
)

pause
