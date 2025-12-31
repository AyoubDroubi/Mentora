# Add docs and scripts folders to Mentora.sln

$slnPath = "Mentora.sln"
$content = Get-Content $slnPath -Raw

# Define the new projects to add
$docsProject = @'
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "docs", "docs", "{A1B2C3D4-E5F6-4718-8901-234567890ABC}"
	ProjectSection(SolutionItems) = preProject
		Server\docs\API-QUICK-REFERENCE.md = Server\docs\API-QUICK-REFERENCE.md
		Server\docs\CHANGES-SUMMARY.md = Server\docs\CHANGES-SUMMARY.md
		Server\docs\DATABASE-SEEDER.md = Server\docs\DATABASE-SEEDER.md
		Server\docs\DATABASE-SETUP-SUMMARY.md = Server\docs\DATABASE-SETUP-SUMMARY.md
		Server\docs\MODULE-1-AUTHENTICATION.md = Server\docs\MODULE-1-AUTHENTICATION.md
		Server\docs\QUICK-START-AR.md = Server\docs\QUICK-START-AR.md
		Server\docs\SWAGGER-AUTO-ENABLE.md = Server\docs\SWAGGER-AUTO-ENABLE.md
		Server\docs\SWAGGER-COMPLETE.md = Server\docs\SWAGGER-COMPLETE.md
		Server\docs\SWAGGER-GUIDE-AR.md = Server\docs\SWAGGER-GUIDE-AR.md
		Server\docs\SWAGGER-INTEGRATION-SUMMARY.md = Server\docs\SWAGGER-INTEGRATION-SUMMARY.md
	EndProjectSection
EndProject
'@

$scriptsProject = @'
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "scripts", "scripts", "{B2C3D4E5-F6A7-4829-9012-345678901BCD}"
	ProjectSection(SolutionItems) = preProject
		Server\scripts\health-check.ps1 = Server\scripts\health-check.ps1
		Server\scripts\health-check.sh = Server\scripts\health-check.sh
	EndProjectSection
EndProject
'@

# Find insertion point (after "Files" project)
$insertionMarker = 'Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Mentora.Api"'
$insertionIndex = $content.IndexOf($insertionMarker)

if ($insertionIndex -gt 0) {
    # Insert the new projects before Mentora.Api
    $newContent = $content.Insert($insertionIndex, "$docsProject`r`n$scriptsProject`r`n")
    
    # Write back to file
    $newContent | Set-Content $slnPath -NoNewline -Encoding UTF8
    
    Write-Host "? Successfully added docs and scripts folders to solution!" -ForegroundColor Green
    Write-Host "?? Added 10 documentation files and 2 script files" -ForegroundColor Cyan
    Write-Host "`nNow reload the solution in Visual Studio to see the changes." -ForegroundColor Yellow
} else {
    Write-Host "? Could not find insertion point in solution file" -ForegroundColor Red
}
