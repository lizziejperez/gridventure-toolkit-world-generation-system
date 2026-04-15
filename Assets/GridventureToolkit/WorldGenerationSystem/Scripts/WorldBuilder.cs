/*
 * WorldBuilder.cs
 * Gridventure Toolkit - World Builder
 * Author: Lizzie Perez
 * Version: 0.0
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates the base logical terrain grid for the world.
/// Handles terrain creation before the terrain is rendered visually to a Tilemap.
/// </summary>
public class WorldBuilder
{
    private WorldGenerationSystemConfig config;
    private List<TerrainTypeData> terrainTypes;
    private TerrainTypeData[,] worldTerrain;

    private struct TerrainNoiseRange
    {
        public float Min;
        public float Max;

        public TerrainNoiseRange(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
    private List<TerrainNoiseRange> terrainNoiseRanges;

    /// <summary>
    /// Creates a new world builder using the provided generation settings.
    /// Initializes the logical terrain grid based on the configured world size.
    /// </summary>
    /// <param name="config">
    /// The shared world generation configuration containing values
    /// such as world width, world height, seed settings, and debug options.
    /// </param>
    /// <param name="terrainTypes">A list of all terrain types to generate.</param>
    public WorldBuilder(WorldGenerationSystemConfig config, List<TerrainTypeData> terrainTypes)
    {
        this.config = config;
        this.terrainTypes = terrainTypes;
        worldTerrain = new TerrainTypeData[config.width, config.height];        
    }

    /// <summary>
    /// Generates the base terrain layout for the world.
    /// Returns if world generation was successful or not.
    /// The current implementation fills the entire terrain grid with default terrain.
    /// </summary>
    public bool Generate()
    {
        // Handle null and empty list of terrain types
        if (terrainTypes == null || terrainTypes.Count == 0)
        {
            return false;
        }

        ApplyNaturalTerrain(); // uses Perlin noise

        // TODO: apply terrain rules
        // TODO: make path carver

        return true;
    }

    /// <summary>
    /// Returns a string of the world terrain 
    /// </summary>
    public string WorldTerrainToString()
    {
        string terrainGrid = "";

        for (int y = config.height - 1; y >= 0; y--)
        {
            for (int x = 0; x < config.width; x++)
            {
                terrainGrid += worldTerrain[x, y].debugSymbol + " ";
            }
            terrainGrid += "\n";
        }

        return terrainGrid;
    }

    // Generate terrains in noise band order with Perlin noise
    private void ApplyNaturalTerrain()
    {
        SetupTerrainRanges();

        // Set the offsets for Perlin noise samples
        System.Random rand = new System.Random(config.seed); // Use config seed to create random number generator
        float offsetX = rand.Next(-100000, 100000);
        float offsetY = rand.Next(-100000, 100000);

        // Apply Perlin noise sampling for procedural terrain generation
        for (int x = 0; x < config.width; x++)
        {
            for (int y = 0; y < config.height; y++)
            {
                // Sample Perlin noise
                float sampleX = (x + offsetX) / config.noiseScale;
                float sampleY = (y + offsetY) / config.noiseScale;
                float perlinNoise = Mathf.PerlinNoise(sampleX, sampleY);

                // Apply coverage threshold
                worldTerrain[x, y] = ApplyCoverageThreshold(perlinNoise);
            }
        }
    }

    private void SetupTerrainRanges()
    {
        // Calculate terrain noise range sum
        float rangeSum = 0f;
        for (int i = 0; i < terrainTypes.Count; i++)
        {
            rangeSum += terrainTypes[i].targetCoverage;
        }

        // Setup terrain noise ranges
        terrainNoiseRanges = new List<TerrainNoiseRange>();
        float start = 0f;
        for (int i = 0; i < terrainTypes.Count; i++)
        {
            float end = start + (terrainTypes[i].targetCoverage / rangeSum);
            terrainNoiseRanges.Add(new TerrainNoiseRange(start, end));
            start = end;
        }
    }

    private TerrainTypeData ApplyCoverageThreshold(float perlinNoise)
    {
        for (int i = 0; i < terrainNoiseRanges.Count; i++)
        {
            if (perlinNoise >= terrainNoiseRanges[i].Min && perlinNoise < terrainNoiseRanges[i].Max)
            {
                return terrainTypes[i];
            }
        }
        return terrainTypes[terrainTypes.Count-1];
    }
}
