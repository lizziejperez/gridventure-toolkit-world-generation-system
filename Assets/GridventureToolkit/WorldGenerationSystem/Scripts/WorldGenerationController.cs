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
    [SerializeField] private WorldGenerationSystemConfig config;
    [SerializeField] private Tilemap worldTileMap;
    [SerializeField] private List<TerrainTypeData> terrainTypes;

    /// <summary>
    /// Initializes world generation when the scene starts.
    /// Applies config settings, generates terrain, logs debug output, and triggers rendering of the world to the Tilemap.
    /// </summary>
    void Start()
    {
        // Set random seed if it's set in the config settings
        if (config.useRandomSeed)
        {
            config.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        // Generate the world terrain with Perlin noise
        WorldBuilder worldBuilder = new WorldBuilder(config, terrainTypes);
        bool worldGenerated = worldBuilder.Generate();

        if (worldGenerated)
        {
            if (config.inDebugMode)
            {
                Debug.Log("Seed: " + config.seed); // print generation seed to debug log 
                Debug.Log(worldBuilder.WorldTerrainToString()); // print the world terrain to debug log 
            }

            // Render the world terrain
            WorldRenderer worldRenderer = new WorldRenderer(config, worldTileMap, worldBuilder);
            worldRenderer.Render();

        } else if (config.inDebugMode)
        {
            Debug.Log("World generation failed.");
        }
    }

    /// <summary>
    /// Reserved for future runtime updates.
    /// Currently unused.
    /// </summary>
    void Update()
    {
        
    }
}
