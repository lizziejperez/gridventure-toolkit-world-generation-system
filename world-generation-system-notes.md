# World Generation System — Notes & Concepts

**Script**: WorldBuilder.cs (in-progress)

## Overview

This system is responsible for generating a tile-based world in Unity using a structured, data-driven approach.

It separates **logical world generation** from **visual rendering**, allowing terrain to be generated using a simple grid while maintaining clean and consistent visuals using Tilemaps and Rule Tiles.

The system builds on an existing tilemap setup with auto-tiling support.

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

## Current Capabilities

- Terrain can be painted using Rule Tiles
- Auto-tiling handles edges and transitions
- Layered terrain system: `grass → path/water → cliffs → features`
- Visual system is cohesive and functional

## System Breakdown

The world generation system will be responsible for:

* Generating terrain using a logical grid
* Applying rules to validate terrain placement
* Converting logical data into visual tiles
* Placing features such as caves and nature objects

Core components:

* Logical terrain grid (`TerrainType[,]`)
* Rule processing system for terrain validation
* Dual-grid style renderer for smoother visuals
* Tilemap painting system
* Feature placement system

## Design Decisions

* Use a ScriptableObject for systems configuration
* Use a **single logical grid** for terrain generation
* Use a **dual-grid renderer** for improved visual transitions
* Separate:
  * world data
  * rule validation
  * rendering logic
* Keep tilemaps as a **visual layer only**
* Build modular systems that can be expanded later

### Structure

- `TerrainType` enum for tile categories (e.g. grass, water, path, cliff)
- `FeatureType` enum for placed world features (e.g. cave, tree, rock, bush)

## Planned Systems

### WorldBuilder

* Generates the base terrain layout (grass, water, path, cliffs)

### TerrainRuleProcessor

* Applies rules such as:
  * Cliff cannot be placed in water
  * Path cannot end in water
  * Water overrides grass
  * Cliff overrides grass

### DualGridRenderer

* Converts logical terrain into visual tile placement
* Handles smoothing and multi-tile updates

### TilemapPainter

* Applies tiles to Unity Tilemaps

### FeaturePlacer

* Places features such as:
  * caves (on cliffs)
  * trees (on grass)
  * rocks and bushes

## Workflow

1. Generate logical terrain grid
2. Apply terrain rules and resolve conflicts
3. Render terrain using Tilemaps and Rule Tiles
4. Place features on valid terrain

## Resources

- Unity Rule Tile documentation: https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@4.3/manual/RuleTile.html?q=rule