/*
 * WorldBuilder.cs
 * Gridventure Toolkit - World Builder
 * Author: Lizzie Perez
 * Version: 0.0
 */
using System;
using UnityEngine;

/// <summary>
/// Generates the base logical terrain grid for the world.
/// Handles terrain creation such as grass, water, paths, and cliffs before the terrain is rendered visually to a Tilemap.
/// </summary>
public class WorldBuilder
{
    public enum TerrainType
    {
        Grass, Water, Path, Cliff
    }

    private TerrainType[,] worldTerrain;
    private WorldGenerationSystemConfig config;

    /// <summary>
    /// Creates a new world builder using the provided generation settings.
    /// Initializes the logical terrain grid based on the configured world size.
    /// </summary>
    /// <param name="config">
    /// The shared world generation configuration containing values
    /// such as world width, world height, seed settings, and debug options.
    /// </param>
    public WorldBuilder(WorldGenerationSystemConfig config)
    {
        this.config = config;
        worldTerrain = new TerrainType[config.width, config.height];
    }

    /// <summary>
    /// Generates the base terrain layout for the world.
    /// The current implementation fills the entire terrain grid with grass.
    /// Future generation steps may layer in water, paths, and cliffs.
    /// </summary>
    public void Generate()
    {
        // TODO: Generate the terrain in order + use seed with Perlin Noise for prodedural generation
        // (1) grass
        // (2) water
        // (3) paths and cliffs

        // Fill terrain with grass
        for (int x = 0; x < config.width; x++)
        {
            for (int y = 0; y < config.height; y++)
            {
                worldTerrain[x, y] = TerrainType.Grass;
            }
        }
    }

    /// <summary>
    /// Prints the current logical terrain grid to the Unity Console using single-character terrain symbols for debugging. 
    /// </summary>
    public void PrintWorldTerrain()
    {
        string terrainGrid = "";

        for (int y = config.height-1;  y >= 0; y--)
        {
            for (int x = 0; x < config.width; x++)
            {
                terrainGrid += TerrainToChar(worldTerrain[x, y]) + " ";
            }
            terrainGrid += "\n";
        }

        Debug.Log(terrainGrid); // prints to Unity's debug log
    }

    private char TerrainToChar(TerrainType type)
    {
        switch (type)
        {
            case TerrainType.Grass: return 'G';
            case TerrainType.Water: return 'W';
            case TerrainType.Path: return 'P';
            case TerrainType.Cliff: return 'C';
            default: return '?';
        }
    }
}
