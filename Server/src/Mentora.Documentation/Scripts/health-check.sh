#!/bin/bash
# Quick Health Check Script for Mentora Database

echo "?? Mentora Database Health Check"
echo "=================================="
echo ""

# Navigate to Infrastructure project
cd "$(dirname "$0")/../src/Mentora.Infrastructure"

echo "1??  Checking EF Tools Version..."
dotnet ef --version
echo ""

echo "2??  Checking Migrations..."
dotnet ef migrations list --startup-project ../Mentora.Api
echo ""

echo "3??  Checking Database Connection..."
dotnet ef database update --startup-project ../Mentora.Api --dry-run
echo ""

echo "4??  Building Project..."
cd ../Mentora.Api
dotnet build
echo ""

echo "? Health Check Complete!"
echo ""
echo "?? Next Steps:"
echo "  - Run: dotnet run (in Mentora.Api folder)"
echo "  - Open: https://localhost:7xxx/swagger"
echo "  - Test: Authentication endpoints"
