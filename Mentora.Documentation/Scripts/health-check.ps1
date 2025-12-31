# Quick Health Check Script for Mentora Database
# PowerShell Version

Write-Host "?? Mentora Database Health Check" -ForegroundColor Cyan
Write-Host "==================================" -ForegroundColor Cyan
Write-Host ""

# Navigate to Infrastructure project
Set-Location "$PSScriptRoot\..\src\Mentora.Infrastructure"

Write-Host "1??  Checking EF Tools Version..." -ForegroundColor Yellow
dotnet ef --version
Write-Host ""

Write-Host "2??  Checking Migrations..." -ForegroundColor Yellow
dotnet ef migrations list --startup-project ..\Mentora.Api
Write-Host ""

Write-Host "3??  Building Project..." -ForegroundColor Yellow
Set-Location ..\Mentora.Api
dotnet build
Write-Host ""

Write-Host "? Health Check Complete!" -ForegroundColor Green
Write-Host ""
Write-Host "?? Next Steps:" -ForegroundColor Cyan
Write-Host "  - Run: dotnet run (in Mentora.Api folder)"
Write-Host "  - Open: https://localhost:7xxx/swagger"
Write-Host "  - Test: Authentication endpoints"
