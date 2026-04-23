/*
 * FeaturePlacer.cs
 * Gridventure Toolkit - Feature Placement System
 * Author: Lizzie Perez
 * Version: 1.0
 */
using UnityEngine;

/// <summary>
/// Places world feature prefabs on generated terrain based on each terrain type's allowed feature list
/// and each feature type's spawn chance.
/// </summary>
public class FeaturePlacer
{
    private System.Random _random;

    /// <summary>
    /// Creates a new feature placer using the provided seed for deterministic placement.
    /// The same seed and terrain data will produce the same feature placement results.
    /// </summary>
    /// <param name="seed">The seed used for deterministic feature placement.</param>
    public FeaturePlacer(int seed)
    {
        // Create a random number generator with the seed
        _random = new System.Random(seed);
    }

    /// <summary>
    /// Places features across the provided terrain grid by checking each cell's allowed features and spawning prefabs based on their configured spawn chances.
    /// Spawned features are parented under the provided transform.
    /// </summary>
    /// <param name="terrainData">The generated terrain grid used to determine valid feature placement.</param>
    /// <param name="parent">The parent transform that will contain all spawned feature instances.</param>
    public void PlaceFeatures(TerrainTypeData[,] terrainData, Transform parent)
    {
        // Get the width and height or terrain data
        int width = terrainData.GetLength(0);
        int height = terrainData.GetLength(1);

        // Calculate the offsets for placement
        // Adjust padding as needed (0.5f is center with terrain tile)
        float paddingX = 0.5f;
        float paddingY = 0.6f;
        float offsetX = -(width / 2) + paddingX;
        float offsetY = -(height / 2) + paddingY;

        // Go over all terrain data
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Determine which feature (if any) should be placed on this terrain cell
                FeatureTypeData featureToPlace = GetFeatureToPlace(terrainData[x, y]);

                // Place feature if there is one
                if (featureToPlace != null)
                {
                    // Spawn the feature
                    Vector3 position = new Vector3(x + offsetX, y + offsetY, parent.position.z);
                    GameObject featureObject = GameObject.Instantiate(featureToPlace.Prefab, position, Quaternion.identity, parent);
                    featureObject.name = featureToPlace.Id; // Use the feature type's id to name the spawned feature
                }
            }
        }
    }

    private FeatureTypeData GetFeatureToPlace(TerrainTypeData terrain)
    {
        foreach (FeatureTypeData feature in terrain.AllowedFeatures)
        {
            if (_random.NextDouble() < feature.SpawnChance)
            {
                return feature;
            }
        }

        return null;
    }
}
