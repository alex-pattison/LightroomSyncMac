# Remove old config/working directories from previous Dev/Debug/Release setup.
# After running this, only Dev (repo) and Beta (%LocalAppData%\LightroomSyncPlus) exist.

$ErrorActionPreference = "Stop"
$appData = [Environment]::GetFolderPath("ApplicationData")
$localAppData = [Environment]::GetFolderPath("LocalApplicationData")

$oldPaths = @(
    (Join-Path $appData "LightroomSyncPlusDev"),
    (Join-Path $appData "LightroomSyncPlus")
)

Write-Host "Removing old instance config folders..."
foreach ($p in $oldPaths) {
    if (Test-Path $p) {
        Write-Host "  Deleting: $p"
        Remove-Item -Recurse -Force $p
    } else {
        Write-Host "  (skip, not found): $p"
    }
}

Write-Host ""
Write-Host "Cleanup done. Current setup:"
Write-Host "  Dev:  Run from H:\GitHub\LightroomSyncMac (config in .lightroom-sync-dev)"
Write-Host "  Beta: Run from $localAppData\LightroomSyncPlus"
Write-Host ""
