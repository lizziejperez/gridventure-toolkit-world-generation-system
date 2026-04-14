/*
 * WorldGenerationController.cs
 * Gridventure Toolkit - World Generation Controller
 * Author: Lizzie Perez
 * Version: 0.0
 */
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
    [SerializeField] Tilemap worldTileMap;
    [SerializeField] GameObject features;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (config.useRandomSeed)
        {
            config.seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }

        WorldBuilder worldBuilder = new WorldBuilder(config);
        worldBuilder.Generate();

        if (config.inDebugMode)
        {
            Debug.Log("Seed: "+config.seed); // print generation seed to debug log 
            worldBuilder.PrintWorldTerrain(); // print the world terrain to debug log            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
