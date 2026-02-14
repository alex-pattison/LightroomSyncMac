# Lightroom Sync+ – Developer Setup Guide

A guide to building and running Lightroom Sync+ for development and testing.

---

## Prerequisites

- **Windows 10/11** (the app uses Windows Forms)
- **.NET 8 SDK** – [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Lightroom Classic** (optional for testing; the app detects when it closes)

---

## Build the App

From the project root:

```powershell
cd h:\GitHub\LightroomSyncPlus
dotnet build LightroomSync.sln
```

Or run it directly:

```powershell
dotnet run --project LightroomSync\LightroomSync.csproj
```

### Run from Visual Studio

Open `LightroomSync.sln` in Visual Studio, then press **F5** or use **Debug > Start Debugging**.

### Launch to System Tray

```powershell
dotnet run --project LightroomSync\LightroomSync.csproj -- tray
```

---

## First Run & Configuration

1. On first launch, the app shows the main window. Click **File > Settings** to configure paths.
2. **Local folder** – where your Lightroom catalogs live (e.g. `C:\Users\<You>\Pictures\Lightroom`).
3. **Sync folder** – shared location (e.g. Google Drive like `G:\My Drive\Lightroom` or `P:\Lightroom`).
4. **Backup folder** – optional; defaults to `Pictures\LightroomBackups` when you choose Backup on replace.
5. **Log folder** – optional; defaults to `Sync folder\Logs`. Use a custom path if needed.

The config is saved to `%AppData%\LightroomSyncPlusDev\config.txt` when you close the app (Debug build). Working files (zips, temp data) go there too. Release builds use `%AppData%\LightroomSyncPlus`.

---

## Setting Up a Dev/Test Catalog

To avoid touching your real catalog:

1. Create a test folder structure, for example:
   ```
   D:\Dev\LightroomTest\
   ├── TestCatalog.lrcat
   ├── TestCatalog.lrcat-data\    (folder, if it exists)
   └── TestCatalog Helper.lrdata\ (folder, if it exists)
   ```

2. Create or copy a minimal Lightroom catalog:
   - Create a new catalog in Lightroom: **File > New Catalog** and save it as `TestCatalog` in `D:\Dev\LightroomTest\`.
   - Or copy an existing `.lrcat` and its data folders into that directory.

3. Create a test network folder:
   ```
   D:\Dev\LightroomSync\Network\
   ```
   (or any local folder that will act as the “network” share).

4. In Lightroom Sync+, open **File > Settings** and set:
   - **Local folder:** `D:\Dev\LightroomTest`
   - **Sync folder:** `D:\Dev\LightroomSync\Network`

5. Test flow:
   - Click **Start Sync**, then open Lightroom and load `TestCatalog`.
   - Make a small change (e.g. add/flag a photo).
   - Close Lightroom.
   - Watch the activity log; the app should zip and upload the catalog.
   - On another machine (or after moving the zip), the app detects the newer version and replaces the local catalog. Use **File > Test out of sync** to simulate this without a second machine.

---

## Project Layout

```
LightroomSyncPlus/
├── LightroomSync.sln
├── LightroomSync/
│   ├── LightroomSync.csproj
│   ├── Program.cs          # Entry point
│   ├── Form1.cs             # Main UI & sync logic
│   ├── Form1.Designer.cs    # UI layout
│   ├── Config.cs            # Local/network paths, settings
│   ├── Status.cs            # Network status, Lightroom detection
│   ├── Utils.cs             # Startup shortcut, paths, misc
│   ├── SettingsDialog.cs    # Settings (paths, backup, log folder)
│   ├── BackupOrDiscardDialog.cs  # Backup vs discard when update available
│   ├── SpinningSyncIcon.cs  # Aperture status icon
│   ├── Alert.cs             # Conflict dialog
│   └── Properties/          # Resources, launch settings
├── LICENSE
├── README.md
└── DEV_SETUP.md            # This file
```

---

## Config File (`config.txt`)

When the app closes, it writes config to `%AppData%\LightroomSyncPlusDev\config.txt` (Debug) or `%AppData%\LightroomSyncPlus\config.txt` (Release):

```json
{
  "LocalFolder": "C:\\Users\\You\\Pictures\\Lightroom",
  "NetworkFolder": "G:\\My Drive\\Lightroom",
  "BackupFolder": "C:\\\\Users\\\\You\\\\Pictures\\\\LightroomBackups",
  "LogFolder": "",
  "ShowActivityLog": false,
  "SkipStartSyncConfirmation": false
}
```

- **LogFolder** – Empty = use `NetworkFolder\Logs`. Set a path for a custom log location.
- **ShowActivityLog** – If true, the activity panel is visible on startup.
- **SkipStartSyncConfirmation** – If true, Start Sync does not show a confirmation dialog.

You can edit this file directly for testing, or change paths in **File > Settings**.

---

## Useful Tips

- **Start Sync** – click to begin watching; the aperture icon spins while syncing.
- **Stop Sync** – click to stop; the icon stops and returns to “ready” state.
- **Launch LR** – when Lightroom is closed, this button launches Lightroom Classic.
- **File > Settings** – configure local folder, sync folder, backup folder, and log folder.
- **File > Show activity log** – toggle the activity panel; useful for debugging.
- **File > Launch at startup** – adds a shortcut to the Windows Startup folder.
- **File > Test out of sync** – simulates “newer catalog available” for testing the Backup/Discard flow.
- **Help > Icon guide** – explains the aperture icon states (dim, spinning, green, yellow, red).
- **Help > Open log folder** – opens the log directory in Explorer. Copy logs when reporting issues.
- **Run with `-- tray`** – start minimized to system tray: `dotnet run --project LightroomSync\LightroomSync.csproj -- tray`

---

## Troubleshooting

### Build errors

- **COM / ResolveComReference** – The project uses P/Invoke for shortcuts; COM references were removed. If you see COM errors, make sure you’re on the latest project version.
- **Missing camera.ico** – The tray icon falls back to a default icon if `camera.ico` is missing. Add `camera.ico` to the project and reference it in the csproj if you want a custom icon.

### Runtime issues

- **“Directory does not exist”** – Ensure Local and Network folders exist and paths are correct.
- **Permission errors** – Run as a user with read/write access to both folders.
- **Lightroom detection** – The app looks for a process named `Lightroom`. If your process name is different, `Status.cs` would need to be updated.

---

## Versioning (beta)

Current version: **0.0.2**

**Source of truth:** `LightroomSync\LightroomSync.csproj` – `<Version>`, `<AssemblyVersion>`, `<FileVersion>`.

On build, the version is copied to `latestVersion.txt` for releases.

### Bumping version before push

```powershell
.\scripts\update-version.ps1 0.0.3
```

Or run without args to be prompted. Updates: csproj, InnoSetup.iss, latestVersion.txt.

---

## Feature Overview

Lightroom Sync+ adds these enhancements over the original LightroomSync:

| Feature | Description |
|---------|-------------|
| Backup on replace | Choose Backup or Discard when a newer catalog is on the network |
| Manual sync control | Start Sync / Stop Sync buttons |
| Launch Lightroom | One-click launch of Lightroom Classic |
| Status icon | Aperture icon with states: dim, spinning, green (LR open), yellow (update), red (error) |
| Settings dialog | Centralized config for local, sync, backup, and log folders |
| Activity log | Toggle-able panel; File > Show activity log |
| Skip confirmation | "Don't show again" on Start Sync dialog |
| Icon guide | Help > Icon guide |
| Open log folder | Help > Open log folder |
| Test out of sync | File > Test out of sync – simulates update flow |
| Dark UI | Modern dark-themed interface |
