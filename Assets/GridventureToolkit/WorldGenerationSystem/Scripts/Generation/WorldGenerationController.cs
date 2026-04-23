/*
 * WorldGenerationController.cs
 * Gridventure Toolkit - World Generation Controller
 * Author: Lizzie Perez
 * Version: 1.0
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Entry point for world generation.
/// Initializes terrain generation, terrain rendering, feature placement, and debug output.
/// </summary>
public class WorldGenerationController : MonoBehaviour
{
    [Header("World Generation Settings")]
    [SerializeField] private WorldGenerationSystemConfig _config;
    [SerializeField] private Tilemap _terrainTilemap;
    [SerializeField] private List<TerrainTypeData> _terrainTypes;
    [SerializeField] private Transform _featuresParent;

    private TerrainRenderer _terrainRenderer;

    /// <summary>
    /// Initializes world generation when the scene starts.
    /// Creates the terrain renderer and generates the initial world, including terrain rendering and feature placement.
    /// </summary>
    void Start()
    {
        // Create the world renderer
        _terrainRenderer = new TerrainRenderer(_config, _terrainTilemap);

        // Generate the world, render the terrain, place features
        GenerateWorld();
    }

    /// <summary>
    /// Generates a new world using the current generation settings, renders it to the Tilemap, and places features on the generated terrain.
    /// </summary>
    /// <remarks>
    /// If random seed generation is enabled, a new seed is assigned before generation.
    /// When debug mode is enabled, the generated seed and terrain layout are logged to the Console.
    /// If terrain generation or terrain rendering fails, feature placement is not performed.
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

        TerrainTypeData[,] worldTerrain = terrainGenerator.GetTerrainData();

        // Print to debug log if in debug mode
        if (_config.InDebugMode)
        {
            // Print the generation seed and terrain debug string
            Debug.Log("Seed: " + _config.Seed);
            Debug.Log(TerrainGenerator.TerrainToDebugString(worldTerrain));
        }
        // Render the new current world terrain
        bool rendered = _terrainRenderer.Render(worldTerrain);

        // Handle failed render
        if (!rendered)
        {
            if (_config.InDebugMode)
            {
                Debug.Log("Terrain rendering failed.");
            }
            return;
        }        

        // Place features
        FeaturePlacer featurePlacer = new FeaturePlacer(_config.Seed);
        featurePlacer.PlaceFeatures(worldTerrain, _featuresParent);
    }
}
