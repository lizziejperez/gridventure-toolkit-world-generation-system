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
    [SerializeField] private string _id;
    [SerializeField] private char _debugSymbol = '?';

    [Header("Generation")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _targetCoverage = 0.0f;

    [Header("Rendering")]
    [SerializeField] private TileBase _tile;

    public string Id => _id;
    public char DebugSymbol => _debugSymbol;
    public float TargetCoverage => _targetCoverage;
    public TileBase Tile => _tile;
}
