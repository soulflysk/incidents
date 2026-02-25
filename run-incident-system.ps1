# Incident Management System Runner
# Bank of Thailand (ก.ล.ต.)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Incident Management System" -ForegroundColor Cyan
Write-Host "  Bank of Thailand (ก.ล.ต.)" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Check if .NET SDK is installed
try {
    $dotnetVersion = & dotnet --version 2>$null
    Write-Host "[INFO] .NET SDK detected: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "[ERROR] .NET SDK not found. Please install .NET 8.0 SDK" -ForegroundColor Red
    Write-Host "Download from: https://dotnet.microsoft.com/download" -ForegroundColor Yellow
    Read-Host "Press Enter to exit"
    exit 1
}

# Clean previous builds
Write-Host "[STEP 1] Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path "bin") {
    Write-Host "  - Removing bin folder..." -ForegroundColor Gray
    Remove-Item -Recurse -Force "bin" -ErrorAction SilentlyContinue
}
if (Test-Path "obj") {
    Write-Host "  - Removing obj folder..." -ForegroundColor Gray
    Remove-Item -Recurse -Force "obj" -ErrorAction SilentlyContinue
}
Write-Host "  - Cleanup completed" -ForegroundColor Green
Write-Host ""

# Restore packages
Write-Host "[STEP 2] Restoring NuGet packages..." -ForegroundColor Yellow
try {
    & dotnet restore --no-cache
    if ($LASTEXITCODE -eq 0) {
        Write-Host "  - Packages restored successfully" -ForegroundColor Green
    } else {
        throw "Package restore failed"
    }
} catch {
    Write-Host "[ERROR] Failed to restore packages" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host ""

# Try different build approaches
Write-Host "[STEP 3] Attempting to build the project..." -ForegroundColor Yellow

$buildAttempts = @(
    @{ Name = "Build without AppHost"; Args = @("build", "--configuration", "Release", "--no-self-contained", "-p:UseAppHost=false") },
    @{ Name = "Build for specific runtime"; Args = @("build", "--configuration", "Release", "--runtime", "win-x64", "--no-self-contained") },
    @{ Name = "Simple build"; Args = @("build", "--configuration", "Debug") },
    @{ Name = "Debug build with verbose"; Args = @("build", "--configuration", "Debug", "--verbosity", "minimal") }
)

$buildSuccess = $false
foreach ($attempt in $buildAttempts) {
    Write-Host "  - Attempt $($buildAttempts.IndexOf($attempt) + 1): $($attempt.Name)..." -ForegroundColor Gray
    try {
        & dotnet $attempt.Args
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  - Build successful!" -ForegroundColor Green
            $buildSuccess = $true
            break
        }
    } catch {
        Write-Host "  - Attempt failed: $($_.Exception.Message)" -ForegroundColor Red
    }
}

if (-not $buildSuccess) {
    Write-Host "[ERROR] All build attempts failed" -ForegroundColor Red
    Write-Host ""
    Write-Host "Possible solutions:" -ForegroundColor Yellow
    Write-Host "1. Run this script as Administrator" -ForegroundColor White
    Write-Host "2. Check folder permissions" -ForegroundColor White
    Write-Host "3. Restart your computer" -ForegroundColor White
    Write-Host "4. Clear NuGet cache: dotnet nuget locals all --clear" -ForegroundColor White
    Write-Host "5. Try running: 'Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser'" -ForegroundColor White
    Read-Host "Press Enter to exit"
    exit 1
}

Write-Host ""
Write-Host "[STEP 4] Starting the application..." -ForegroundColor Yellow
Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Application is starting..." -ForegroundColor Cyan
Write-Host "  Access URL: http://localhost:5000" -ForegroundColor Green
Write-Host "  Access URL: https://localhost:7001" -ForegroundColor Green
Write-Host "  Press Ctrl+C to stop the application" -ForegroundColor Yellow
Write-Host "========================================" -ForegroundColor Cyan
Write-Host ""

# Run the application
try {
    & dotnet run --urls "http://localhost:5000;https://localhost:7001" --no-build
} catch {
    Write-Host "[ERROR] Failed to start application: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "Application stopped." -ForegroundColor Yellow
Read-Host "Press Enter to exit"
