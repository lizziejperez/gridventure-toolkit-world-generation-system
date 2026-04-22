/*
 * TerrainTypeData.cs
 * Gridventure Toolkit - Terrain Type Data
 * Author: Lizzie Perez
 * Version: 0.0
 */
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

/// <summary>
/// Defines a terrain type used by the world generation, rendering, and feature placement systems.
/// Stores the terrain identity, debug symbol, generation settings, rendered tile, and allowed feature types.
/// </summary>
[CreateAssetMenu(fileName = "TerrainTypeData", menuName = "Scriptable Objects/Gridventure Toolkit/Terrain Type Data")]
public class TerrainTypeData : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string _id;
    [SerializeField] private char _debugSymbol = '?';

    [Header("Generation")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _targetCoverage = 0.0f;

    [Header("Rendering")]
    [SerializeField] private TileBase _tile;

    [Header("Features")]
    [SerializeField] private List<FeatureTypeData> _allowedFeatures;

    /// <summary>
    /// Gets the unique identifier for this terrain type.
    /// </summary>
    public string Id => _id;

    /// <summary>
    /// Gets the character used to represent this terrain type in debug terrain output.
    /// </summary>
    public char DebugSymbol => _debugSymbol;

    /// <summary>
    /// Gets the target relative coverage of this terrain type during procedural generation.
    /// Higher values give this terrain type a larger share of the generated terrain.
    /// </summary>
    public float TargetCoverage => _targetCoverage;

    /// <summary>
    /// Gets the Tilemap tile used to visually render this terrain type.
    /// </summary>
    public TileBase Tile => _tile;

    /// <summary>
    /// Gets the list of feature types that are allowed to spawn on this terrain type.
    /// </summary>
    public List<FeatureTypeData> AllowedFeatures => _allowedFeatures;
}
