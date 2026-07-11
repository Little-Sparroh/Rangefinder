# Rangefinder

A BepInEx mod for MycoPunk that displays the distance to whatever you're aiming at in the center of the HUD.

## Features

- **Distance Readout**: Shows the range (in meters) to the surface under your crosshair
- **Configurable Max Range**: Set how far the rangefinder will measure
- **Toggleable HUD**: Enable or disable the display via config
- **Hot Reload**: Config changes are picked up automatically while the game is running

## Getting Started

### Dependencies

* MycoPunk (base game)
* [BepInEx](https://github.com/BepInEx/BepInEx) - Version 5.4.2403 or compatible
* .NET Framework 4.8
* [HarmonyLib](https://github.com/pardeike/Harmony) (included via NuGet)

### Building/Compiling

1. Clone this repository
2. Open the solution file in Visual Studio, Rider, or your preferred C# IDE
3. Build the project in Release mode to generate the .dll file

Alternatively, use dotnet CLI:
```bash
dotnet build --configuration Release
```

### Installing

**Via Thunderstore (Recommended)**:
1. Download and install via Thunderstore Mod Manager
2. The mod will be automatically installed to the correct directory

**Manual Installation**:
1. Place the built `Rangefinder.dll` in your `<MycoPunk Directory>/BepInEx/plugins/` folder

### Executing program

The mod loads automatically through BepInEx when the game starts. Check the BepInEx console for loading confirmation messages.

## Configuration

Access mod settings through the BepInEx configuration file at `<MycoPunk Directory>/BepInEx/config/sparroh.rangefinder.cfg`. Options include:

- **EnableRangefinderHUD**: Toggle the rangefinder display on or off (default: true)
- **RangefinderMaxRange**: Maximum measurement distance in meters (default: 500)

## Help

* **Mod not loading?** Verify BepInEx is installed correctly and check console logs for errors
* **No distance shown?** Confirm the HUD is enabled in the config and that you are in a mission
* **UI elements missing?** Confirm mod version compatibility and verify no other mods are interfering

## Authors

- Sparroh

## License

This project is licensed under the MIT License - see the LICENSE file for details
