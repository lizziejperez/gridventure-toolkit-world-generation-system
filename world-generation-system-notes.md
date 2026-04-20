# World Generation System — Notes & Concepts

**Scripts**: `TerrainTypeData.cs`, `WorldGenerationSystemConfig.cs`, `WorldGenerationController.cs`, `WorldBuilder.cs`, `WorldRenderer.cs`, `WorldSaveData.cs`, `WorldSaveSystem.cs`, `WorldSaveLoadController.cs`

## Overview

This system is responsible for generating a tile-based world in Unity using a structured, data-driven approach.

It separates **logical world generation**, **visual rendering**, and **data persistence**, allowing terrain to be generated using a simple grid while maintaining clean and consistent visuals using Tilemaps and Rule Tiles.

The system builds on an existing Tilemap setup with auto-tiling support and now includes region-based Tilemap save/load support.

## Current Implementation

The following systems are already implemented:

* Grid and Tilemap setup in Unity
* Tile Palette workflow for terrain painting
* Rule Tile systems for terrain rendering:
  * Grass with weighted random flower variation
  * Path using a 4-tile auto-tiling setup
  * Water using a 13-tile auto-tiling setup
* Manual cliff / mountain edge tiles for elevation boundaries and cave placement
* Feature assets and variations:
  * Cave entrance
  * Tree variants
  * Rock variants
  * Bush
* Procedural terrain generation using Perlin noise
* World generation controller flow for generation and rendering
* Region-based Tilemap save/load using palette-indexed save data

## Current Capabilities

- Terrain can be painted using Rule Tiles
- Auto-tiling handles edges and transitions
- Layered terrain system: `grass → path/water → cliffs → features`
- Procedural terrain can be generated using Perlin noise
- Generated terrain can be rendered directly to a Tilemap
- Saved Tilemap regions can be serialized and restored

## System Breakdown

The world generation system will be responsible for:

* Generating terrain using a logical grid
* Applying coverage-based terrain assignment through Perlin noise
* Converting logical data into visual tiles
* Saving and loading Tilemap regions as reusable world data
* Supporting future feature placement such as caves and nature objects

Core components:

* Logical terrain grid (`TerrainTypeData[,]`)
* Perlin noise terrain assignment
* Tilemap renderer
* Tilemap save/load system
* Future feature placement system

## Save & Load System

The save/load system is responsible for persisting Tilemap data independently of procedural generation.

### Approach

* Tilemaps are treated as the source of truth for saved visual data
* Tiles are stored as indices into a predefined tile palette
* A rectangular region is saved using:
  * origin (bottom-left Tilemap cell)
  * width
  * height

### Save Data Structure

* `OriginX`, `OriginY` — starting Tilemap cell position
* `Width`, `Height` — dimensions of the saved region
* `WorldTileIndexes[]` — flattened array of tile palette indices
  * `-1` represents an empty cell

### Responsibilities

* `WorldSaveSystem`
  * Handles serialization and deserialization
  * Converts Tilemap tiles to palette indices when saving
  * Converts palette indices back into Tilemap tiles when loading

* `WorldSaveLoadController`
  * Stores save configuration in the Inspector
  * Defines the file name id, save origin, save width, save height, Tilemap, and tile palette
  * Provides public save/load methods for runtime use

### Design Notes

* TileBase references are not serialized directly
* Palette indices provide stable, lightweight save data
* Save/load is decoupled from procedural generation config
* Save/load currently operates on rectangular Tilemap regions

## Design Decisions

* Use a ScriptableObject for world generation configuration
* Use `TerrainTypeData` ScriptableObjects to define terrain identity, generation coverage, and rendering tile
* Use a logical terrain grid for generation
* Use Tilemaps as the primary rendering layer
* Allow Tilemaps to act as the persistence source for saved visual data
* Keep generation, rendering, and persistence as separate responsibilities
* Build modular systems that can be expanded later

### Structure

- `TerrainTypeData` ScriptableObject for terrain type data
- `TerrainNoiseRange` struct for Min and Max Perlin noise values assigned to a terrain type
- `WorldSaveData` class for saved Tilemap region data
- `FeatureType` enum for placed world features (e.g. cave, tree, rock, bush)

## Planned Systems

### WorldBuilder

* Expand terrain generation further as needed
* Continue supporting coverage-based terrain generation

### WorldRenderer

* Converts logical terrain into visual tile placement
* Applies generated terrain to Unity Tilemaps

### TerrainRuleProcessor

* Applies rules such as:
  * Cliff cannot be placed in water
  * Path cannot end in water
  * Water overrides grass
  * Cliff overrides grass

### FeaturePlacer

* Places features such as:
  * caves (on cliffs)
  * trees (on grass)
  * rocks and bushes

## Workflow

1. Generate logical terrain grid
2. Assign terrain using Perlin noise and configured terrain coverage
3. Render generated terrain using Tilemaps
4. Optionally save a Tilemap region using the save/load system
5. Load saved Tilemap data back into the Tilemap when needed
<!-- 6. Expand later with terrain rules and feature placement -->

## Resources

- Unity Rule Tile documentation: https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@4.3/manual/RuleTile.html?q=rule
- https://unity.com/how-to/scriptableobject-based-enums
- https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Tilemaps.Tilemap.SetTiles.html
- https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html