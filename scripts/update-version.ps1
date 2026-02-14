# Update version across all files. Run before pushing.
# Usage: .\update-version.ps1 0.0.3
#    or: .\update-version.ps1  (prompts for version)

param(
    [Parameter(Mandatory=$false)]
    [string]$NewVersion
)

$ErrorActionPreference = "Stop"
$scriptDir = Split-Path -Parent $PSCommandPath
$root = if (Test-Path (Join-Path $scriptDir "..\LightroomSync")) {
    (Resolve-Path (Join-Path $scriptDir "..")).Path
} else {
    throw "Run from repo root or scripts folder; LightroomSync not found."
}

if (-not $NewVersion) {
    $current = (Get-Content (Join-Path $root "latestVersion.txt") -Raw).Trim()
    $NewVersion = Read-Host "Enter new version (current: $current)"
}

# Validate semver-style
if ($NewVersion -notmatch '^\d+\.\d+\.\d+') {
    Write-Error "Version should be in form X.Y.Z (e.g. 0.0.3)"
}

$csproj = Join-Path $root "LightroomSync\LightroomSync.csproj"
$iss = Join-Path $root "InnoSetup.iss"
$latestTxt = Join-Path $root "latestVersion.txt"

# Update csproj
$csprojContent = Get-Content $csproj -Raw
$csprojContent = $csprojContent -replace '<Version>\d+\.\d+\.\d+</Version>', "<Version>$NewVersion</Version>"
$csprojContent = $csprojContent -replace '<AssemblyVersion>\d+\.\d+\.\d+</AssemblyVersion>', "<AssemblyVersion>$NewVersion</AssemblyVersion>"
$csprojContent = $csprojContent -replace '<FileVersion>\d+\.\d+\.\d+</FileVersion>', "<FileVersion>$NewVersion</FileVersion>"
Set-Content $csproj -Value $csprojContent -NoNewline

# Update InnoSetup
$issContent = Get-Content $iss -Raw
$issContent = $issContent -replace '#define MyAppVersion "\d+\.\d+\.\d+"', "#define MyAppVersion `"$NewVersion`""
Set-Content $iss -Value $issContent -NoNewline

# Update latestVersion.txt
Set-Content $latestTxt -Value $NewVersion -NoNewline

Write-Host "Updated version to $NewVersion in:"
Write-Host "  - LightroomSync.csproj"
Write-Host "  - InnoSetup.iss"
Write-Host "  - latestVersion.txt"
