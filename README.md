# Lightroom Sync+ DEV


A tool to manage your Lightroom Classic catalogs across multiple Windows computers. Fork of [LightroomSync](https://github.com/software-2/LightroomSync) by Anthony Bryan, with backup-on-replace and other enhancements.

Lightroom catalogs are designed to be single-user, and Lightroom won't even let you run your catalog from a network folder, even though all your photos are on that NAS. But if you're like me, you want to work on your photos in multiple locations (such as your laptop on the couch for triaging, and your main workstation for more advanced editing). This means having to manually copy your catalog around, and that's a pain. Well no more! This program will save your catalog(s) to a network folder, and automatically update your local catalogs if a newer version is on the network. 

[INSERT SCREENSHOT]

## Features

### Core sync (from original LightroomSync)
- Automatically uploads a zip of your catalog to your network folder as soon as you close Lightroom.
- Automatically replaces your catalogs with the newest version on the network.
- Prevents you from having Lightroom open on multiple machines at once to avoid conflicts and overwriting work.
- Can launch at startup in the system tray – never think about this problem again!

### Enhancements in Lightroom Sync+
- **Backup on replace** – When a newer catalog is available, choose to backup your current catalog or discard it before updating.
- **Manual sync control** – Start Sync / Stop Sync button to begin or end watching for changes.
- **Launch Lightroom** – One-click launch of Lightroom Classic from the app.
- **Status icon** – Visual aperture icon shows sync state (dim = ready, spinning = syncing, green = LR open, yellow = update available, red = error).
- **Settings dialog** – Centralized configuration for local folder, sync folder, backup folder, and custom log folder.
- **Activity log** – Toggle-able activity panel (File > Show activity log) showing sync events; can be hidden for a compact view.
- **Skip confirmation** – Option to skip the “Start Sync?” dialog (via “Don’t show again”).
- **Icon guide** – Help > Icon guide explains each icon state.
- **Open log folder** – Help > Open log folder opens the log directory in Explorer.
- **Test out of sync** – File > Test out of sync simulates the “newer catalog available” flow for testing.
- **Modern dark UI** – Redesigned interface with dark theme and compact layout.

## Setup Instructions
- Build from source, or use the latest installer on the [Releases](https://github.com/alex-pattison/LightroomSyncPlus/releases) page.
- Open **File > Settings** and configure:
  - **Local folder** – where your Lightroom catalogs are stored on this machine.
  - **Sync folder** – common network location (e.g. Google Drive, Dropbox, NAS) that all machines can access.
  - **Backup folder** – where to save backups when replacing catalogs (optional; default is `Pictures\LightroomBackups`).
- Click **Start Sync** to begin watching for changes.
- If you want this to run all the time, select **File > Launch at startup**. It will run in the system tray.
- With sync running, open Lightroom Classic, then close it. After detecting Lightroom has closed, it will zip and upload your catalogs.
- Launch this program on another machine, configure the same sync folder, and click Start Sync. It will grab every catalog from the network.
