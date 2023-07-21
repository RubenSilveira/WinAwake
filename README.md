# WinAwake

Prevents your Windows computer from going to sleep or hibernate, your
screen from going blank, your screensaver from starting and even other
applications from detecting user inactivity.

It is a very simple utility looking like a play symbol that sits on your
system tray: red when active, grey when idle.

It installs and runs under the user's context, so no administrative
rights are necessary.

## Requirements

- Targeting .NET Framework 4.6 (through 4.8.1), with compatibility and
ease of install in mind:
	- Runtime support installed by default on Windows 10+;
	- Runtime support installable on Vista, 7, 8 and 8.1.

## Development / Build

- Application built with Visual Studio 2022 Community:
	- Installation:
		- .NET desktop development
		- .NET Framework 4.6 targeting pack
	- Extensions:
		- HeatWave (WiX v4).
- Developed and tested on Windows 11 x64 only;

## Contributing

- Contributions are always welcome!
- To do:
	- Battery power detection, timers, hotkeys, configurable action time
interval, command line options, different action modes and a simple about
box! :grinning:

## License

Published under [The
MIT License](https://github.com/RubenSilveira/WinAwake/blob/main/LICENSE).

## Changelog

### v3 (2023-07-21)
	Features:
	- Locked session detection
	- Update checker

### v2 (2023-07-16)
	Features:
	- Configurable startup state.
	Bugs:
	- Registry reading on startup was causing a crash.

### v1 (2023-07-16)
	Features:
	- Initial version.