# Gridventure Toolkit: World Generation System (Unity C#)

🚧 **Status: In Development (v0.3 - Procedural Terrain Generation and Tile Save/Load Implemented)**

A modular Unity world generation system designed for creating procedural environments using grid-based generation, auto-tiling, and Perlin noise.

This system is part of the **Gridventure Toolkit**, focused on building beginner-friendly, reusable systems for Unity game development.

## Overview

This project provides a foundation for procedural world generation in Unity using a tile-based workflow.

It includes a working Tilemap system with Rule Tiles for terrain rendering, procedural terrain generation using Perlin noise, and a save/load system that allows rectangular Tilemap regions to be persisted and restored using palette-based tile indexing.

The system is designed to separate logical world generation, visual rendering, and persistence into modular, easy-to-understand components.

## Scripts

* `TerrainTypeData.cs`
* `WorldGenerationSystemConfig.cs`
* `WorldGenerationController.cs`
* `WorldBuilder.cs`
* `WorldRenderer.cs`
* `WorldSaveData.cs`
* `WorldSaveSystem.cs`
* `WorldSaveLoadController.cs`

## Current Features

* Grid and Tilemap setup in Unity
* Tile Palette workflow for painting terrain
* Path Rule Tile (auto-tiling)
* Water Rule Tile (auto-tiling)
* Grass system with random variation (flowers)
* Layered terrain approach (grass, path, water)
* Procedural terrain generation using Perlin noise
* Configurable world generation settings
* Runtime world generation through a controller-driven workflow

### Save & Load System

* Save rectangular Tilemap regions using tile palette indices
* Load and reconstruct Tilemap regions from saved data
* Configurable save origin, width, and height
* JSON-based save files using `Application.persistentDataPath`

## Planned Features

* Feature placement system (trees, rocks, bushes, caves)
* Terrain validation and terrain rule processing
* More advanced rendering options (dual-grid)
* Modular architecture for easy extension

## Current Progress

* Project initialized
* Repository structure set up
* Tilemap system implemented
* Rule Tile system integrated
* Procedural terrain generation working with Perlin noise
* Terrain rendering working in-scene
* Initial world save/load system implemented

## Design Goals

* Beginner-friendly and easy to understand
* Modular and reusable
* Clean separation of systems
* Consistent with other Gridventure Toolkit systems
* Scalable for more advanced generation systems later

## Future Systems

This system will connect with:

* Feature placement tools
* Advanced terrain rule processing
* Smoother or more advanced rendering systems
* Expanded generation systems

## Notes

This system is still in development and is not yet ready for production use.

Documentation, demos, and features will continue to be expanded as development progresses.

## 💼 Freelance & Support

Need help with Unity systems or procedural generation?

https://www.fiverr.com/lizziejperez
