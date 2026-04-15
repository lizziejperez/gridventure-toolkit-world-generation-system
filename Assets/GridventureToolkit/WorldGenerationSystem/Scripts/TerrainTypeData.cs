/*
 * TerrainTypeData.cs
 * Gridventure Toolkit - Terrain Type Data
 * Author: Lizzie Perez
 * Version: 0.0
 */
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TerrainTypeData", menuName = "Scriptable Objects/Gridventure Toolkit/Terrain Type Data")]
public class TerrainTypeData : ScriptableObject
{
    [Header("Identity")]
    public string id;
    public char debugSymbol = '?';

    [Header("Generation")]
    [Range(0.0f, 1.0f)]
    public float targetCoverage = 0.0f;

    [Header("Rendering")]
    public TileBase tile;
}
