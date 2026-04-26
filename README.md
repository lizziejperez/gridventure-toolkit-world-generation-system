# Gridventure Toolkit: World Generation System (Unity C#)

**Version:** 1.0

A beginner-friendly, modular Unity system for generating procedural 2D worlds using Tilemaps, Perlin noise, and ScriptableObject-driven design.

Part of the **Gridventure Toolkit** — reusable systems for building 2D top-down games in Unity.

## Features

* Procedural terrain generation using Perlin noise
* Tilemap-based rendering with Rule Tile support
* Data-driven terrain and feature setup (ScriptableObjects)
* Deterministic world generation using seeds
* Feature placement system (trees, rocks, etc.)
* Save & load system using tile palette indexing
* Clean, modular architecture (easy to extend)

## How It Works (Quick Overview)

1. **Terrain is generated** using Perlin noise
2. **Terrain is rendered** to a Unity Tilemap
3. **Features are placed** based on terrain rules
4. *(Optional)* World can be saved and loaded

For a full breakdown of how all scripts work together, see:

[World Generation System Documentation](world-generation-system-notes.md)

## How to Use (Beginner Friendly)

### 1. Import into Your Project

* Import the scripts into your Unity project
* Make sure you have:
  * A **Grid**
  * A **Tilemap** (for terrain)

### 2. Create Terrain Types

Create ScriptableObjects:

**Right Click → Create → Gridventure Toolkit → Terrain Type Data**

For each terrain:

* Set **ID** (e.g. Grass, Water, Path)
* Assign a **Tile**
* Set **Target Coverage** (how often it appears)
* Assign allowed **Feature Types** (optional)

### 3. Create Feature Types (Optional)

**Right Click → Create → Gridventure Toolkit → Feature Type Data**

For each feature:

* Set **ID** (e.g. Tree, Rock)
* Assign a **Prefab**
* Set **Spawn Chance** (0–1)

### 4. Create World Config

**Right Click → Create → Gridventure Toolkit → World Generation System Config**

Set:

* Width / Height
* Noise Scale
* Seed (or enable random seed)
* Debug mode (optional)

### 5. Set Up the Scene

Add **WorldGenerationController** to a GameObject:

Assign:

* Config
* Terrain Tilemap
* Terrain Types list
* Features Parent (empty GameObject)

### 6. Press Play

* A world will generate automatically at runtime
* Terrain renders to Tilemap
* Features spawn on valid tiles

## Save & Load (Optional)

### Setup

Add **WorldSaveLoadController** to a GameObject:

Assign:

* Tilemap
* Tile Palette (important!)
* Features Parent
* Feature Types list

### Usage

* Trigger Save/Load via input or UI
* Worlds are stored as JSON files
* Uses `Application.persistentDataPath`

## Design Goals

* Beginner-friendly
* Modular and reusable
* Clean separation of systems
* Easy to expand for future features

## Notes

* Designed for **2D top-down games**
* Uses a **centered grid system**
* Works best with Unity Tilemaps and Rule Tiles

## 💼 Freelance & Support

Need help with Unity systems or procedural generation?

https://www.fiverr.com/lizziejperez

