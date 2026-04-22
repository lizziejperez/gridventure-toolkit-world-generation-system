/*
* WorldGenerationSystemConfig.cs
* Gridventure Toolkit - World Generation System Configuration
* Author: Lizzie Perez
* Version: 0.0
*/
using UnityEngine;

/// <summary>
/// Stores shared world height, width, seed, and debug settings for world generation systems.
/// </summary>
[CreateAssetMenu(fileName = "WorldGenerationSystemConfig", menuName = "Scriptable Objects/Gridventure Toolkit/World Generation System Config")]
public class WorldGenerationSystemConfig : ScriptableObject
{
    [Header("World Generation System Configuration Settings")]
    [SerializeField] private int _width = 18;
    [SerializeField] private int _height = 10;    
    [SerializeField] private bool _useRandomSeed = true;
    [SerializeField] private int _seed = 12345;
    [SerializeField] private float _noiseScale = 10f;

    [Header("Debug Mode Settings")]
    [SerializeField] private bool _inDebugMode = false;

    public int Height => _height;
    public int Width => _width;
    public bool UseRandomSeed
    {
        get { return _useRandomSeed; }
        set { _useRandomSeed = value; }
    }
    public int Seed
    {
        get { return _seed; }
        set { _seed = value; }
    }
    public float NoiseScale => _noiseScale;
    public bool InDebugMode => _inDebugMode;
}
