# Fix Solution Explorer - Nest docs and scripts under Server

Write-Host "Fixing Solution Explorer structure..." -ForegroundColor Cyan

# 1. Close Visual Studio if open
Write-Host "`n[1/5] Checking Visual Studio processes..." -ForegroundColor Yellow
$vsProcesses = Get-Process | Where-Object {$_.ProcessName -like "*devenv*"}
if ($vsProcesses) {
    Write-Host "  ??  Visual Studio is running. Please close it first!" -ForegroundColor Red
    Write-Host "  Press any key after closing Visual Studio..." -ForegroundColor Yellow
    $null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
}

# 2. Backup solution file
Write-Host "`n[2/5] Backing up Mentora.sln..." -ForegroundColor Yellow
Copy-Item "Mentora.sln" "Mentora.sln.backup" -Force
Write-Host "  ? Backup created: Mentora.sln.backup" -ForegroundColor Green

# 3. Read solution file
Write-Host "`n[3/5] Reading solution file..." -ForegroundColor Yellow
$solutionContent = Get-Content "Mentora.sln" -Raw

# 4. Fix NestedProjects section
Write-Host "`n[4/5] Fixing NestedProjects section..." -ForegroundColor Yellow

# Find the NestedProjects section and add docs and scripts under Server
$nestedProjectsPattern = '(GlobalSection\(NestedProjects\) = preSolution\r?\n)([\s\S]*?)(EndGlobalSection)'

if ($solutionContent -match $nestedProjectsPattern) {
    $before = $Matches[1]
    $existing = $Matches[2]
    $after = $Matches[3]
    
    # Add docs and scripts nesting (A1B2C3D4 = docs, B2C3D4E5 = scripts, DAD03CFC = Server)
    $newNesting = @"
$existing		{A1B2C3D4-E5F6-4718-8901-234567890ABC} = {DAD03CFC-D5FA-AC94-41C7-9ABAB326F09E}
		{B2C3D4E5-F6A7-4829-9012-345678901BCD} = {DAD03CFC-D5FA-AC94-41C7-9ABAB326F09E}
"@
    
    $solutionContent = $solutionContent -replace $nestedProjectsPattern, "$before$newNesting`r`n	$after"
}

# 5. Save solution file
Write-Host "`n[5/5] Saving fixed solution file..." -ForegroundColor Yellow
$solutionContent | Set-Content "Mentora.sln" -NoNewline
Write-Host "  ? Solution file updated!" -ForegroundColor Green

# 6. Clean cache
Write-Host "`n[6/6] Cleaning Visual Studio cache..." -ForegroundColor Yellow
if (Test-Path ".vs") {
    Remove-Item -Recurse -Force ".vs" -ErrorAction SilentlyContinue
    Write-Host "  ? Removed .vs folder" -ForegroundColor Green
}

Get-ChildItem -Include bin,obj -Recurse -Directory | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
Write-Host "  ? Removed bin/obj folders" -ForegroundColor Green

# Done
Write-Host "`n" -NoNewline
Write-Host "??????????????????????????????????????????????????????" -ForegroundColor Cyan
Write-Host "?  " -NoNewline -ForegroundColor Cyan
Write-Host "? Solution Explorer Fixed Successfully! " -NoNewline -ForegroundColor Green
Write-Host "        ?" -ForegroundColor Cyan
Write-Host "??????????????????????????????????????????????????????" -ForegroundColor Cyan

Write-Host "`nNext Steps:" -ForegroundColor Yellow
Write-Host "  1. Open Mentora.sln in Visual Studio" -ForegroundColor White
Write-Host "  2. You should now see:" -ForegroundColor White
Write-Host "     ??? Server" -ForegroundColor Cyan
Write-Host "         ??? docs (with all .md files)" -ForegroundColor Green
Write-Host "         ??? scripts (with .ps1 and .sh files)" -ForegroundColor Green
Write-Host "         ??? src (all projects)" -ForegroundColor Green

Write-Host "`n?? Tip: If still empty, right-click Solution ? Reload Solution" -ForegroundColor Yellow
Write-Host "`nPress any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
