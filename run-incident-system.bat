@echo off
title Incident Management System - Bank of Thailand
color 0A
echo.
echo ========================================
echo   Incident Management System
echo   Bank of Thailand (ก.ล.ต.)
echo ========================================
echo.

REM Check if .NET SDK is installed
dotnet --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] .NET SDK not found. Please install .NET 8.0 SDK
    echo Download from: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo [INFO] .NET SDK detected
echo.

REM Clean previous builds
echo [STEP 1] Cleaning previous builds...
if exist "bin" (
    echo   - Removing bin folder...
    rmdir /s /q "bin" 2>nul
)
if exist "obj" (
    echo   - Removing obj folder...
    rmdir /s /q "obj" 2>nul
)
echo   - Cleanup completed
echo.

REM Restore packages
echo [STEP 2] Restoring NuGet packages...
dotnet restore --no-cache
if %ERRORLEVEL% NEQ 0 (
    echo [ERROR] Failed to restore packages
    pause
    exit /b 1
)
echo   - Packages restored successfully
echo.

REM Try different build approaches
echo [STEP 3] Attempting to build the project...

REM Approach 1: Build without AppHost
echo   - Attempt 1: Building without AppHost...
dotnet build --configuration Release --no-self-contained -p:UseAppHost=false
if %ERRORLEVEL% EQU 0 (
    echo   - Build successful!
    goto :run_app
)

REM Approach 2: Build for specific runtime
echo   - Attempt 2: Building for specific runtime...
dotnet build --configuration Release --runtime win-x64 --no-self-contained
if %ERRORLEVEL% EQU 0 (
    echo   - Build successful!
    goto :run_app
)

REM Approach 3: Simple build
echo   - Attempt 3: Simple build...
dotnet build --configuration Debug
if %ERRORLEVEL% EQU 0 (
    echo   - Build successful!
    goto :run_app
)

echo [ERROR] All build attempts failed
echo.
echo Possible solutions:
echo 1. Run this script as Administrator
echo 2. Check folder permissions
echo 3. Restart your computer
echo 4. Clear NuGet cache: dotnet nuget locals all --clear
pause
exit /b 1

:run_app
echo.
echo [STEP 4] Starting the application...
echo.
echo ========================================
echo   Application is starting...
echo   Access URL: http://localhost:5000
echo   Access URL: https://localhost:7001
echo   Press Ctrl+C to stop the application
echo ========================================
echo.

REM Try to run with different URLs
dotnet run --urls "http://localhost:5000;https://localhost:7001" --no-build

echo.
echo Application stopped.
pause
