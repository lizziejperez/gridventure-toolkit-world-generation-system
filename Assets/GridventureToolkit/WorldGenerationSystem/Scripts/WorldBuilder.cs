/*
 * WorldBuilder.cs
 * Gridventure Toolkit - World Builder
 * Author: Lizzie Perez
 * Version: 0.0
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Generates the base logical terrain grid for the world.
/// Handles terrain creation before the terrain is rendered visually to a Tilemap.
/// </summary>
public class WorldBuilder
{
    private WorldGenerationSystemConfig config;
    private List<TerrainTypeData> terrainTypes;
    private TerrainTypeData[,] worldTerrain;

    /// <summary>
    /// Represents a range of Perlin noise values assigned to a terrain type.
    /// </summary>
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
    /// Converts the generated terrain grid into a readable string format.
    /// Useful for debugging terrain generation in the console.
    /// Each terrain type is represented by its debug symbol.
    /// </summary>
    /// <returns>
    /// A string representation of the terrain grid, with rows separated by new lines.
    /// </returns>
    public string WorldTerrainToString()
    {
        // Prevent changes with width and height during method call
        int width = config.width;
        int height = config.height;

        string terrainGrid = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                terrainGrid += worldTerrain[x, y].debugSymbol + " ";
            }
            terrainGrid += "\n";
        }

        return terrainGrid;
    }

    /// <summary>
    /// Converts the internal 2D terrain grid into an array of TileBase objects.
    /// This format is required by Unity's Tilemap.SetTiles() method.
    /// </summary>
    /// <remarks>
    /// The array is ordered row by row (left to right, bottom to top), using the index formula: index = y * width + x.
    /// This ordering must match the renderer's position array.
    /// </remarks>
    /// <returns>
    /// A flattened array of TileBase tiles representing the world terrain.
    /// </returns>
    public TileBase[] GetWorldTerrain()
    {
        // Prevent changes with width and height during method call
        int width = config.width;
        int height = config.height;

        // Convert grid to an array for the renderer
        TileBase[] terrainArray = new TileBase[width * height];
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = y * width + x;
                terrainArray[i] = worldTerrain[x, y].tile;
            }
        }

        return terrainArray;
    }

    /// <summary>
    /// Generates terrain using Perlin noise based on the configured noise scale and seed.
    /// Each tile is assigned a terrain type based on noise value thresholds.
    /// </summary>
    /// <remarks>
    /// This method populates the worldTerrain grid by sampling Perlin noise and mapping values to terrain types using coverage thresholds.
    /// </remarks>
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

    /// <summary>
    /// Calculates normalized noise ranges for each terrain type based on their target coverage.
    /// These ranges are used to map Perlin noise values to terrain types.
    /// </summary>
    /// <remarks>
    /// The sum of all terrain target coverages is normalized to 1.0, and each terrain type is assigned a proportional slice of the noise range.
    /// </remarks>
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

    /// <summary>
    /// Determines which terrain type corresponds to a given Perlin noise value.
    /// </summary>
    /// <param name="perlinNoise">
    /// A value between 0 and 1 generated by Perlin noise.
    /// </param>
    /// <returns>
    /// The TerrainTypeData that falls within the matching noise range.
    /// </returns>
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
