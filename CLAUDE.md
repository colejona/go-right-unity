# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

"Go Right" is a 2D platformer/action game built with Unity 6000.3.13f1. The project uses Universal Render Pipeline (URP) with the 2D renderer.

## Unity Version & Tooling

- **Unity**: 6000.3.13f1
- **Render Pipeline**: URP v17.3.0 with 2D Renderer (`Assets/Settings/Renderer2D.asset`)
- **Input System**: Unity New Input System v1.19.0 (`Assets/InputSystem_Actions.inputactions`)
- **IDE**: Rider or Visual Studio (both configured)

## Build & Development Commands

Unity projects are built through the Unity Editor GUI, not the command line. To open the project, launch Unity Hub and open this folder.

For headless/CI builds:
```bash
# Windows
"C:\Program Files\Unity\Hub\Editor\6000.3.13f1\Editor\Unity.exe" -batchmode -quit -projectPath . -buildWindows64Player Build/GoRight.exe -logFile build.log

# Run tests headlessly
"C:\Program Files\Unity\Hub\Editor\6000.3.13f1\Editor\Unity.exe" -batchmode -quit -projectPath . -runTests -testPlatform EditMode -testResults results.xml
```

## Project Structure

```
Assets/
├── Scenes/           # Unity scenes (SampleScene.unity is the only scene)
├── Settings/         # URP renderer and pipeline settings
├── InputSystem_Actions.inputactions  # Input bindings (Player action map)
└── Scripts/          # C# scripts (to be created)
Packages/
└── manifest.json     # Package dependencies
ProjectSettings/      # Unity project configuration
```

## Input System

The `InputSystem_Actions.inputactions` file defines the **Player** action map:

| Action    | Type    | Bindings                     |
|-----------|---------|------------------------------|
| Move      | Vector2 | WASD / Left Stick            |
| Look      | Vector2 | Mouse Delta / Right Stick    |
| Jump      | Button  | Space / South Button         |
| Sprint    | Button  | Left Shift / Left Stick Press|
| Crouch    | Button  | C / East Button              |
| Attack    | Button  | Mouse Left / West Button     |
| Interact  | Button (Hold) | E / North Button        |

Use the generated C# class `InputSystem_Actions` to access these bindings in scripts.

## Key Packages

- `com.unity.2d.animation` — skeletal 2D animation
- `com.unity.2d.tilemap` + `com.unity.2d.tilemap.extras` — level tilemaps
- `com.unity.2d.aseprite` — import Aseprite sprite sheets directly
- `com.unity.2d.psdimporter` — import layered PSD files as sprites
- `com.unity.timeline` — cutscenes and sequenced events
- `com.unity.test-framework` — EditMode and PlayMode unit tests

## Architecture Notes

The project is in early development (no C# scripts exist yet). When adding scripts:

- The main scene (`Assets/Scenes/SampleScene.unity`) uses an orthographic camera at depth -1 with a Global Light 2D — keep this in mind for any lighting or camera scripts.
- The render pipeline is 2D URP; use `Light2D` components, not standard `Light` components.
- Color space is Linear; ensure textures and materials are configured accordingly.
- The game targets 1920×1080 landscape.
