# WinAwake

Prevents your Windows computer from going to sleep or hibernate, your
screen from going blank, your screensaver from starting and even other
applications from detecting that you are inactive.

It is a very simple utility looking like a play symbol that sits on your
system tray: red when active, grey when idle.

It pauses itself when on battery power and when locked.

## Installation

Install packages are hosted on GitHub. You can download the latest one
from [here](https://github.com/RubenSilveira/WinAwake/releases/latest).

Administrative rights are not needed, as both the installation and the
application run under the user's unelevated privileges.

## Requirements

.NET Framework 4.6 (through 4.8.1):
- Is installed by default on Windows 10+;
- Can be installed on Vista, 7, 8 and 8.1.

## Development / Build

Developed and tested on Windows 11 x64 only, using Visual Studio 2022
Community:
- Installation:
	- .NET desktop development
	- .NET Framework 4.6 targeting pack
- Extensions:
	- HeatWave (WiX v4).

## License

Published under [The
MIT License](https://github.com/RubenSilveira/WinAwake/blob/main/LICENSE).

## Changelog

### v4 (2023-07-22)
	Features:
	- Battery power detection;
	- Link to about page.
    Changes:
    - Update checker is now manual only.

### v3 (2023-07-21)
	Features:
	- Locked session detection;
	- Update checker.

### v2 (2023-07-16)
	Features:
	- Configurable startup state.
	Bugs:
	- Registry reading on startup was causing a crash.

### v1 (2023-07-16)
	Features:
	- Initial version.