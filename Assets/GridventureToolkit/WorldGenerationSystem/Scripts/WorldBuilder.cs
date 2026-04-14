/*
 * WorldBuilder.cs
 * Gridventure Toolkit - World Builder
 * Author: Lizzie Perez
 * Version: 0.0
 */
using UnityEngine;

/// <summary>
/// Generates the base terrain layout (grass, water, path, cliffs)
/// </summary>
public class WorldBuilder : MonoBehaviour
{
    [Header("World Build Settings")]
    [SerializeField] private int worldGridHeight = 10;
    [SerializeField] private int worldGridWidth = 10;

    // Define terrain types
    enum TerrainType
    {
        Grass, Water, Path, Cliff
    }

    // Define feature types
    enum FeatureType
    {
        Rock, Tree, Bush, Cave
    }

    private TerrainType[,] worldTerrainGrid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
