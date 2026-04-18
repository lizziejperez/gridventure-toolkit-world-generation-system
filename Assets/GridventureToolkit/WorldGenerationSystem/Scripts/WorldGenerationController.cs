/*
 * WorldGenerationController.cs
 * Gridventure Toolkit - World Generation Controller
 * Author: Lizzie Perez
 * Version: 0.0
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Entry point for world generation.
/// Initializes generation systems, triggers terrain creation, and handles debug output.
/// </summary>
public class WorldGenerationController : MonoBehaviour
{
    [Header("World Generation Settings")]
    [SerializeField] private WorldGenerationSystemConfig _config;
    [SerializeField] private Tilemap _worldTileMap;
    [SerializeField] private List<TerrainTypeData> _terrainTypes;

    private TerrainTypeData[,] _currentWorldTerrain;
    private WorldRenderer _worldRenderer;

    /// <summary>
    /// Initializes world generation when the scene starts.
    /// Applies config settings, generates terrain, logs debug output, and triggers rendering of the world to the Tilemap.
    /// </summary>
    void Start()
    {
        _worldRenderer = new WorldRenderer(_config, _worldTileMap);
        GenerateWorld();
    }

    public void GenerateWorld()
    {
        // Set random seed if it's set in the config settings
        if (_config.UseRandomSeed)
        {
            _config.Seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        // Generate the world terrain with Perlin noise
        WorldBuilder worldBuilder = new WorldBuilder(_config, _terrainTypes);
        bool worldGenerated = worldBuilder.Generate();

        // Handle world generation failure
        if (!worldGenerated)
        {
            if (_config.InDebugMode)
            {
                Debug.Log("World generation failed.");
            }
            return;
        }

        // Update current world terrain stored
        _currentWorldTerrain = worldBuilder.GetWorldTerrainData();

        if (_config.InDebugMode)
        {
            Debug.Log("Seed: " + _config.Seed); // print generation seed to debug log 
            Debug.Log(worldBuilder.WorldTerrainToString()); // print the world terrain to debug log
        }

        // Render the new current world terrain
        _worldRenderer.Render(_currentWorldTerrain);
    }

    /// <summary>
    /// Reserved for future runtime updates.
    /// Currently unused.
    /// </summary>
    void Update()
    {
        
    }
}
