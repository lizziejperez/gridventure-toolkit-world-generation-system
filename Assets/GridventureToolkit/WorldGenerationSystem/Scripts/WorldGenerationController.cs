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
    [SerializeField] private Tilemap _terrainTilemap;
    [SerializeField] private List<TerrainTypeData> _terrainTypes;

    private TerrainRenderer _terrainRenderer;

    /// <summary>
    /// Initializes world generation when the scene starts.
    /// Applies config settings, generates terrain, logs debug output, and triggers rendering of the world to the Tilemap.
    /// </summary>
    void Start()
    {
        // Create the world renderer
        _terrainRenderer = new TerrainRenderer(_config, _terrainTilemap);

        // Generate the world and render it
        GenerateWorld();
    }

    /// <summary>
    /// Generates a new world using the current generation settings and renders it to the Tilemap.
    /// </summary>
    /// <remarks>
    /// If random seed generation is enabled, a new seed is assigned before generation.
    /// When debug mode is enabled, the generated seed and terrain layout are logged to the Console.
    /// If generation fails, no rendering is performed.
    /// </remarks>
    public void GenerateWorld()
    {
        // Set random seed if it's set in the config settings
        if (_config.UseRandomSeed)
        {
            _config.Seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        // Generate the world terrain with Perlin noise
        TerrainGenerator terrainGenerator = new TerrainGenerator(_config, _terrainTypes);
        bool terrainGenerated = terrainGenerator.Generate();

        // Handle terrain generation failure
        if (!terrainGenerated)
        {
            if (_config.InDebugMode)
            {
                Debug.Log("Terrain generation failed.");
            }
            return;
        }

        if (_config.InDebugMode)
        {
            Debug.Log("Seed: " + _config.Seed); // print generation seed to debug log 
            Debug.Log(terrainGenerator.TerrainToString()); // print the world terrain to debug log
        }

        // Render the new current world terrain
        bool rendered = _terrainRenderer.Render(terrainGenerator.GetTerrainData());

        // Handle failed render
        if (!rendered)
        {
            if (_config.InDebugMode)
            {
                Debug.Log("Terrain rendering failed.");
            }
            return;
        }
    }
}
