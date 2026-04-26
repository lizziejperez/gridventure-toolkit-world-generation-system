# World Generation System (v1) — Notes & Concepts

## Table of Contents

* [Overview](#overview)
* [Core Flow](#core-flow)
* [File Breakdown](#file-breakdown)
  * [WorldGenerationController](#worldgenerationcontroller)
  * [WorldGenerationSystemConfig](#worldgenerationsystemconfig)
  * [TerrainGenerator](#terraingenerator)
  * [TerrainTypeData](#terraintypedata)
  * [TerrainRenderer](#terrainrenderer)
  * [FeaturePlacer](#featureplacer)
  * [FeatureTypeData](#featuretypedata)
* [Save & Load System (Optional)](#save--load-system-optional)
  * [WorldSaveLoadController](#worldsaveloadcontroller)
  * [WorldSaveSystem](#worldsavesystem)
  * [WorldSaveData](#worldsavedata)
  * [WorldFeatureSaveData](#worldfeaturesavedata)
* [System Design Notes](#system-design-notes)
* [Summary](#summary)
* [References](#references)

## Overview

The World Generation System is responsible for creating a procedural 2D terrain, rendering it to a Tilemap, placing features, and optionally saving/loading the world state.

The system is modular and built around three main phases:

1. **Generate terrain (data)**
2. **Render terrain (visuals)**
3. **Place features (game objects)**

## Core Flow

```
WorldGenerationController
    ↓
TerrainGenerator → TerrainTypeData
    ↓
TerrainRenderer
    ↓
FeaturePlacer → FeatureTypeData
```

Optional:

```
WorldSaveLoadController → WorldSaveSystem → WorldSaveData
```

## File Breakdown

### `WorldGenerationController.cs`

**Entry point for the system** 

* Initializes and runs world generation on scene start
* Controls the full pipeline: generate → render → place features
* Applies seed logic and debug output

### `WorldGenerationSystemConfig.cs`

**Defines world generation settings (ScriptableObject)** 

* World size (width/height)
* Seed and randomization settings
* Noise scale for terrain generation
* Debug mode toggle

Used by all core systems for consistency.

### `TerrainGenerator.cs`

**Creates the logical terrain grid** 

* Uses Perlin noise to generate terrain
* Maps noise values to terrain types using coverage ranges
* Outputs a `TerrainTypeData[,]` grid

This is **data-only** (no visuals).

### `TerrainTypeData.cs`

**Defines a terrain type (ScriptableObject)** 

* Unique ID and debug symbol
* Target coverage (generation weight)
* Tile for rendering
* Allowed features for placement

Acts as the **bridge between generation, rendering, and features**.

### `TerrainRenderer.cs`

**Converts terrain data into visuals** 

* Takes terrain grid and renders it to a Tilemap
* Uses centered positioning based on world size
* Batches tile placement for performance

Handles **visual output only**.

### `FeaturePlacer.cs`

**Places world features on terrain** 

* Iterates through terrain grid
* Checks allowed features per terrain type
* Spawns prefabs based on spawn chance
* Uses seed for deterministic placement

Adds **gameplay elements** to the world.

### `FeatureTypeData.cs`

**Defines a feature type (ScriptableObject)** 

* Unique ID
* Prefab reference
* Spawn chance

Used by both **feature placement and save/load systems**.

## Save & Load System (Optional)

### `WorldSaveLoadController.cs`

**Handles player-triggered save/load** 

* Connects input/UI to save system
* Defines save region and tile palette
* Passes data to save system

### `WorldSaveSystem.cs`

**Core save/load logic** 

* Saves Tilemap as palette indices (not raw tiles)
* Saves placed features by ID and position
* Loads and reconstructs terrain + features

### `WorldSaveData.cs`

**Serialized world data** 

* World dimensions
* Flattened tile indices
* Placed feature data

### `WorldFeatureSaveData.cs`

**Serialized feature instance** 

* Feature ID
* World position

## System Design Notes

* **Deterministic generation**: Same seed = same world + features
* **Data-driven design**: Terrain and features use ScriptableObjects
* **Separation of concerns**:

  * Generation = data
  * Rendering = visuals
  * Features = gameplay
* **Centered coordinate system**: World is positioned around origin

## Summary

The Gridventure Toolkit World Generation System (v1) provides a modular pipeline for:

* Procedural terrain generation
* Tilemap-based rendering
* Feature spawning
* Save/load support

It is designed to be **extensible**, **deterministic**, and **easy to integrate** into 2D top-down games.

## References

The following Unity documentation and resources were used in the development of this system:

- Unity Rule Tile documentation: https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@4.3/manual/RuleTile.html?q=rule
- Tilemap.SetTiles API: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Tilemaps.Tilemap.SetTiles.html
- Application.persistentDataPath: https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html
- AI-assisted tools (such as ChatGPT) were used to support development, code structuring, and documentation.