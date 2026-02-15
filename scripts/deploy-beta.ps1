# Build and deploy Beta to %LocalAppData%\LightroomSyncPlus
# Beta runs from there and uses its own config (separate from Dev).
# Uses self-contained publish so Beta runs without requiring a global .NET install.

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $PSCommandPath
$root = Resolve-Path (Join-Path $scriptDir "..")
$betaDir = Join-Path $env:LOCALAPPDATA "LightroomSyncPlus"

Write-Host "Publishing self-contained Beta..."
Set-Location $root
dotnet publish LightroomSync\LightroomSync.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o (Join-Path $root "publish\beta") --nologo -v q
if ($LASTEXITCODE -ne 0) { exit 1 }

$srcDir = Join-Path $root "publish\beta"

Write-Host "Deploying Beta to $betaDir"
if (!(Test-Path $betaDir)) { New-Item -ItemType Directory -Path $betaDir -Force | Out-Null }
Copy-Item "$srcDir\*" -Destination $betaDir -Force -Recurse
Get-ChildItem $betaDir -Name | ForEach-Object { Write-Host "  $_" }

Write-Host ""
Write-Host "Beta deployed. Launch with:"
Write-Host "  Start-Process `"$betaDir\LightroomSyncPlus.exe`""
Write-Host ""
