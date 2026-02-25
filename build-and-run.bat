@echo off
echo Building Incident Management System...
echo.

REM Clean previous builds
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"

REM Restore packages
echo Restoring NuGet packages...
dotnet restore

REM Build the project
echo Building project...
dotnet build --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
    echo.
    echo Starting the application...
    echo The application will be available at: https://localhost:7001
    echo Press Ctrl+C to stop the application
    echo.
    dotnet run --urls "https://localhost:7001"
) else (
    echo.
    echo Build failed! Please check the error messages above.
    pause
)
