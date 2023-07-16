# WinAwake

Prevents your Windows computer from going to sleep or hibernate, your screen from
going blank, your screensaver from starting and even other applications from
detecting user inactivity.

It it a very simple utility looking like a play symbol that sits on your system
tray: red when active, grey when idle.

## Requirements

- Targeting .NET Framework 4.6 (through 4.8.1), with compatibility and ease of
install in mind:
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
	- Configurable startup state, battery power detection, lock status detection,
timers, hotkeys, configurable action time interval, application configuration per
settings & per command line, different action modes and a simple about box! :)

## License

This code is published under [The
MIT License](https://github.com/RubenSilveira/WinAwake/blob/main/LICENSE).

## Changelog

### Version 1 (2023-07-16)

Features:
* Initial version.