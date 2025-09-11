# Exiter

Lightweight Windows tray app that prevents your PC from sleeping or auto-locking by periodically nudging the mouse cursor by 1px and back. Runs in the system tray with an Exit menu option.

- Platform: Windows 10/11 (x64)
- Technology: .NET 8, Windows Forms
- Version: v1.0.0

## Why

Some environments aggressively put machines to sleep or set presence to Away when there is no input. Exiter simulates minimal mouse movement every 30 seconds to keep the session active without interfering with your work.

## How it works

- The app is a background Windows Forms process with no visible window.
- A tray icon appears in the notification area.
- Every 30 seconds, it calls `SendInput` (user32) to move the cursor by 1 pixel in one direction and then back immediately.
- No keyboard events, no clicks, and no network activity.

Code reference (simplified):
- Timer interval: 30,000 ms (30s)
- Input API: `SendInput` from `user32.dll`
- Tray menu: right-click the icon → Exit

## Download

Get the latest signed build from GitHub Releases:

- Latest: https://github.com/indigocc/Exiter/releases/latest

Download the asset named similar to:
- `Exiter-1.0.0-win-x64.zip`

Then extract and run `Exiter.exe`.

## Usage

1. Run `Exiter.exe`.
2. A tray icon labeled “Exiter” will appear.
3. The app will automatically perform a tiny mouse jiggle every 30 seconds.
4. To quit: right-click the tray icon → Exit.

Notes:
- The jiggle is minimal and should not interfere with normal use.
- Some apps/platforms may detect “activity” differently; cursor movement is a common and lightweight signal but doesn’t cover every case.

## Start on login (optional)

If you want Exiter to start automatically:

- Startup Folder (simplest)
  - Press Win+R → type `shell:startup` → Enter
  - Copy a shortcut to `Exiter.exe` into that folder

- Task Scheduler (more control)
  - Open Task Scheduler → Create Task…
  - Triggers: At log on
  - Actions: Start a program → `Exiter.exe`
  - Conditions: Uncheck “Start the task only if the computer is on AC power” if desired

## Build from source

Requirements:
- .NET 8 SDK: https://dotnet.microsoft.com/download

Project structure:
- Source is under the `Exiter` subfolder in this repository.

Build:
```powershell
# From repository root
dotnet build Exiter/Exiter.csproj -c Release
```

Publish (Win-x64):
```powershell
# Produces a redistributable folder under bin/Release/net8.0-windows/win-x64/publish
dotnet publish Exiter/Exiter.csproj -c Release -r win-x64 --self-contained true
```

You can also create a ZIP of the published output to attach to Releases.

## Development details

- Hidden form, minimized on start, not shown in taskbar.
- Tray icon uses the app executable icon if available; falls back to the default application icon.
- Single context menu command: Exit.
- Interval is currently fixed at 30 seconds.

## Privacy & Security

- Exiter does not collect or transmit any data.
- It only sends local mouse-move input events to Windows.

## Troubleshooting

- If you don’t see the tray icon, Windows may have hidden it:
  - Click the “Show hidden icons” arrow in the system tray
  - Drag Exiter’s icon onto the visible area if you want it pinned
- If antivirus flags the app, add an allow-list rule or build from source.
- If the app doesn’t prevent sleep/lock in your setup, consider verifying system power/policy settings, or using Task Scheduler to keep the session active via other means.

## License

No license file is currently included. If you intend to redistribute or modify, please add a license appropriate to your needs (for example, MIT).
