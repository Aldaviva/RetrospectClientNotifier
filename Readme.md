RetrospectClientNotifier
===

Notify the user when their computer is being backed up with Retrospect.

This used to work out of the box, but Windows 8 removed the ability for background services to show UI, so the dialog boxes never appear.

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="true" levels="1,2" -->

- [Requirements](#requirements)
- [Installation](#installation)
- [Appearance](#appearance)

<!-- /MarkdownTOC -->

<a id="requirements"></a>
## Requirements
- Windows 8 or later
- [.NET Framework 4.8 runtime](https://dotnet.microsoft.com/download/dotnet-framework/net48)
- Retrospect Client
    - Tested with [7.7.114](https://www.retrospect.com/en/support/archived)
- Retrospect Server
    - Tested with [7.7.620](https://www.retrospect.com/en/support/archived)

<a id="installation"></a>
## Installation
1. Go to this repository's latest [Release](https://github.com/Aldaviva/RetrospectClientNotifier/releases/latest).
1. Download `retroeventhandler.exe` to your Retrospect Client installation directory.
    - The default installation directory is `%ProgramFiles(x86)%\Retrospect\Retrospect Client`.

<a id="appearance"></a>
## Appearance

<a id="when-a-backup-starts"></a>
### When a backup starts

A native Windows toast notification is shown for a few seconds.

![Starting backup](https://i.imgur.com/g8mmFz4.png)

<a id="while-a-backup-is-running"></a>
### While a backup is running

A tray icon is visible in the notification area. The tooltip shows the time the backup started.

![Tray icon](https://i.imgur.com/NcydDft.png)

If you don't want this icon to appear, you can right-click on it and hide it, either once or forever.

<a id="after-a-backup-finishes"></a>
### After a backup finishes

Another toast notification appears, and it describes the quantity and size of the files that were backed up, as well as how long it took.

![Backup complete](https://i.imgur.com/NLiZbq2.png)

If you missed a notification or want to see it again, you can see the most recent one in the Windows Action Center (Win+A, or click the ![Action Center](https://www.tenforums.com/attachments/tutorials/115951d1485975474-open-action-center-windows-10-a-new_notification.png) Action Center button to the right of the clock).

![Action Center](https://i.imgur.com/veBf7DM.png)