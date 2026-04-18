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
    private WorldGenerationSystemConfig _config;
    private List<TerrainTypeData> _terrainTypes;
    private TerrainTypeData[,] _worldTerrain;

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
    private List<TerrainNoiseRange> _terrainNoiseRanges;

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
        _config = config;
        _terrainTypes = terrainTypes;
        _worldTerrain = new TerrainTypeData[config.Width, config.Height];        
    }

    /// <summary>
    /// Generates the base terrain layout for the world.
    /// Returns if world generation was successful or not.
    /// The current implementation fills the entire terrain grid with default terrain.
    /// </summary>
    public bool Generate()
    {
        // Handle null and empty list of terrain types
        if (_terrainTypes == null || _terrainTypes.Count == 0)
        {
            return false;
        }

        ApplyNaturalTerrain(); // uses Perlin noise

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
        int width = _config.Width;
        int height = _config.Height;

        string terrainGrid = "";

        for (int y = height - 1; y >= 0; y--)
        {
            for (int x = 0; x < width; x++)
            {
                terrainGrid += _worldTerrain[x, y].DebugSymbol + " ";
            }
            terrainGrid += "\n";
        }

        return terrainGrid;
    }

    /// <summary>
    /// Retruns the generated world terrain.
    /// </summary>
    /// <returns>A 2D array of the terrain grid.</returns>
    public TerrainTypeData[,] GetWorldTerrainData()
    {
        return _worldTerrain;
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
        System.Random rand = new System.Random(_config.Seed); // Use config seed to create random number generator
        float offsetX = rand.Next(-100000, 100000);
        float offsetY = rand.Next(-100000, 100000);

        // Apply Perlin noise sampling for procedural terrain generation
        for (int x = 0; x < _config.Width; x++)
        {
            for (int y = 0; y < _config.Height; y++)
            {
                // Sample Perlin noise
                float sampleX = (x + offsetX) / _config.NoiseScale;
                float sampleY = (y + offsetY) / _config.NoiseScale;
                float perlinNoise = Mathf.PerlinNoise(sampleX, sampleY);

                // Apply coverage threshold
                _worldTerrain[x, y] = ApplyCoverageThreshold(perlinNoise);
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
        for (int i = 0; i < _terrainTypes.Count; i++)
        {
            rangeSum += _terrainTypes[i].TargetCoverage;
        }

        // Setup terrain noise ranges
        _terrainNoiseRanges = new List<TerrainNoiseRange>();
        float start = 0f;
        for (int i = 0; i < _terrainTypes.Count; i++)
        {
            float end = start + (_terrainTypes[i].TargetCoverage / rangeSum);
            _terrainNoiseRanges.Add(new TerrainNoiseRange(start, end));
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
        for (int i = 0; i < _terrainNoiseRanges.Count; i++)
        {
            if (perlinNoise >= _terrainNoiseRanges[i].Min && perlinNoise < _terrainNoiseRanges[i].Max)
            {
                return _terrainTypes[i];
            }
        }
        return _terrainTypes[_terrainTypes.Count-1];
    }
}
