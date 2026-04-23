/*
 * FeatureTypeData.cs
 * Gridventure Toolkit - Feature Type Data
 * Author: Lizzie Perez
 * Version: 1.0
 */
using UnityEngine;

/// <summary>
/// Defines a placeable world feature type used by terrain feature placement and save/load systems.
/// Stores the feature identity and the prefab used to spawn it in the world.
/// </summary>
[CreateAssetMenu(fileName = "FeatureTypeData", menuName = "Scriptable Objects/Gridventure Toolkit/Feature Type Data")]
public class FeatureTypeData : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string _id;

    [Header("Prefab")]
    [SerializeField] private GameObject _prefab;

    [Header("Placement")]
    [Range(0.0f, 1.0f)]
    [SerializeField] private float _spawnChance = 0.0f;

    /// <summary>
    /// Gets the unique identifier for this feature type.
    /// </summary>
    public string Id => _id;

    /// <summary>
    /// Gets the prefab used to spawn this feature type in the world.
    /// </summary>
    public GameObject Prefab => _prefab;

    /// <summary>
    /// Gets the probability of this feature spawning on a valid terrain tile.
    /// Value is between 0.0 (never) and 1.0 (always).
    /// </summary>
    public float SpawnChance => _spawnChance;
}
