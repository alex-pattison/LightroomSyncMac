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

1. On first launch, the app shows the main window with **Local Folder** and **Network Folder** fields.
2. **Local Folder** – where your Lightroom catalogs live (e.g. `C:\Users\<You>\Pictures\Lightroom`).
3. **Network Folder** – shared location (e.g. Google Drive path like `G:\My Drive\Lightroom` or `P:\Lightroom`).

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

4. In Lightroom Sync+, set:
   - **Local Folder:** `D:\Dev\LightroomTest`
   - **Network Folder:** `D:\Dev\LightroomSync\Network`

5. Test flow:
   - Open Lightroom and load `TestCatalog`.
   - Make a small change (e.g. add/flag a photo).
   - Close Lightroom.
   - Watch the Events log; the app should zip and upload the catalog.
   - Click the “Local Folder” label to force an upload manually.
   - On another “machine” (or after moving the zip), the app should detect the newer version and replace the local catalog.

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
│   ├── Alert.cs             # Conflict dialog
│   └── Properties/         # Resources, launch settings
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
  "AutoCheckForUpdates": true,
  "BackupFolder": "C:\\\\Users\\\\You\\\\Pictures\\\\LightroomBackups"
}
```

You can edit this file directly for testing, or change paths in the UI.

---

## Useful Tips

- **Click “Local Folder”** – triggers an immediate upload (no need to open/close Lightroom).
- **Events log** – shows sync activity and errors; copy it when reporting issues.
- **File > Launch At Startup** – adds a shortcut to the Windows Startup folder.
- **File > Run At Startup** – run the app with `tray` so it starts minimized to the system tray.

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

## Next Steps

Once you have a working dev instance and test catalog, you can:

1. Add the **backup vs discard** behavior for out-of-date catalogs.
2. Plan **cross-platform support** (Mac/PC) by abstracting paths and platform-specific code.
