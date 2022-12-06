Set-Location ..
$targetDir = "../" + (Split-Path -Path (Get-Location) -Leaf) + "-Release"
Write-Host "Making Release Package in: $targetDir"

if (Test-Path -Path $targetDir)
{
    Write-Host "`nCleaning up previous release package"
    Remove-Item -Path $targetDir -Recurse -Force | Out-Null
}

New-Item $targetDir -ItemType Directory | Out-Null

Write-Host "`nCopying Files"
Copy-Item `
    -Path (Join-Path (Get-Location) "*") `
    -Destination $targetDir `
    -Recurse `
    -Exclude "Source", ".gitignore", ".git"

Write-Host "`nRemoving `"_DEV_`" from \About\About.xml"
((Get-Content -path "$targetDir\About\About.xml" -Raw) -replace '_DEV_', '') | `
    Set-Content -Path "$targetDir\About\About.xml"

Write-Host "`nCopied the following:"
Get-ChildItem -Path $targetDir -Recurse -Name -File
