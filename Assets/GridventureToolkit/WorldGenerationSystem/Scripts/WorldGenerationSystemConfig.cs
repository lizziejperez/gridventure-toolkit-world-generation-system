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
[CreateAssetMenu(fileName = "WorldGenerationSystemConfig", menuName = "Scriptable Objects/WorldGenerationSystemConfig")]
public class WorldGenerationSystemConfig : ScriptableObject
{
    [Header("World Generation System Configuration Settings")]
    [SerializeField] public int height = 10;
    [SerializeField] public int width = 18;
    [SerializeField] public bool useRandomSeed = true;
    [SerializeField] public int seed = 12345;

    [Header("Debug Mode Settings")]

    /// <summary>
    /// Enables additional debug logging for world generation systems.
    /// </summary>
    [SerializeField] public bool inDebugMode = false;
}
