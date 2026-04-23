/*
* WorldSaveSystem.cs
* Gridventure Toolkit - World Save System
* Author: Lizzie Perez
* Version: 1.0
*/
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles saving and loading a rectangular Tilemap region using tile palette indices.
/// </summary>
/// <remarks>
/// Tiles are stored as indices into a configured tile palette rather than as direct Unity asset references.
/// A saved value of -1 represents an empty Tilemap cell.
/// </remarks>
public static class WorldSaveSystem
{
    /// <summary>
    /// Saves a rectangular region of a Tilemap to a JSON file using tile palette indices.
    /// </summary>
    /// <param name="worldWidth">The number of columns to save.</param>
    /// <param name="worldHeight">The number of rows to save.</param>
    /// <param name="saveFileName">The save file name without extension.</param>
    /// <param name="targetTilemap">The Tilemap to read tiles from.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert TileBase references into saved indices.</param>
    /// <param name="featuresParent">Parent transform containing feature instances to save and load.</param>
    /// <param name="featureTypes">List of valid feature types used to validate and recreate saved features.</param>
    /// <remarks>
    /// Tiles are stored in row-major order using the index formula: index = y * width + x.
    /// A value of -1 is stored for empty cells.
    /// </remarks>
    public static void Save(int worldWidth, int worldHeight, string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        // Handle invalid save data
        if (!HasValidSaveInputs(worldWidth, worldHeight, saveFileName, targetTilemap, tilePalette, featuresParent, featureTypes))
        {
            return;
        }

        // Initialize the save data
        WorldSaveData saveData = new WorldSaveData
        {
            Width = worldWidth,
            Height = worldHeight,
            TilePaletteIndices = new int[worldWidth * worldHeight]
        };

        // Store the world tilemap to save data
        SaveTerrainTiles(saveData, worldWidth, worldHeight, targetTilemap, tilePalette);

        // Optionally store the world features to save data
        if (featuresParent != null && featureTypes != null && featureTypes.Count > 0)
        {
            SavePlacedFeatures(saveData, worldWidth, worldHeight, featuresParent, featureTypes);
        } else
        {
            saveData.PlacedFeatures = new WorldFeatureSaveData[0];
        }

        // Write the save data as JSON to the save file
        WriteSaveFile(saveData, saveFileName);
    }

    /// <summary>
    /// Loads a saved Tilemap region from a JSON file and applies it to the provided Tilemap.
    /// </summary>
    /// <param name="saveFileName">The save file name without extension.</param>
    /// <param name="targetTilemap">The Tilemap to populate with saved tiles.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert saved indices back into TileBase references.</param>
    /// <param name="featuresParent">Parent transform containing feature instances to save and load.</param>
    /// <param name="featureTypes">List of valid feature types used to validate and recreate saved features.</param>
    /// <remarks>
    /// Saved tiles are restored starting at the saved origin stored in the save file.
    /// A saved value of -1 clears the corresponding Tilemap cell.
    /// </remarks>
    public static void Load(string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        // Handle missing expected inputs
        if (!HasValidLoadInputs(saveFileName, targetTilemap, tilePalette, featuresParent, featureTypes))
        {
            return;
        }

        // Get the save data
        WorldSaveData saveData = ReadSaveFile(saveFileName);
        if (saveData == null)
        {
            return;
        }

        // Validate expected data
        if (saveData.TilePaletteIndices == null)
        {
            Debug.LogWarning("Load failed: saved tile index array is missing.");
            return;
        }
        if (saveData.Width <= 0 || saveData.Height <= 0)
        {
            Debug.LogWarning("Load failed: saved world width and height must be greater than 0.");
            return;
        }
        if (saveData.TilePaletteIndices.Length != (saveData.Width * saveData.Height))
        {
            Debug.LogWarning("Load failed: saved tile count does not match saved width and height.");
            return;
        }

        // Load the saved terrain tiles to target tilemap
        LoadTerrainTiles(saveData, targetTilemap, tilePalette);

        // Load the placed features to features parent
        LoadPlacedFeatures(saveData, featuresParent, featureTypes);
    }

    /// <summary>
    /// Builds the full save file path for a world save JSON file.
    /// </summary>
    /// <param name="saveFileName">The save file name without extension.</param>
    /// <returns>
    /// The absolute path to the save file in Application.persistentDataPath.
    /// </returns>
    private static string GetPath(string saveFileName)
    {
        return Path.Combine(Application.persistentDataPath, saveFileName + ".json");
    }

    private static bool HasValidSaveInputs(int worldWidth, int worldHeight, string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        // Handle missing save data
        if (string.IsNullOrWhiteSpace(saveFileName))
        {
            Debug.LogWarning("Save failed: save file name is empty.");
            return false;
        }

        if (targetTilemap == null)
        {
            Debug.LogWarning("Save failed: missing world tilemap to save.");
            return false;
        }

        if (tilePalette == null || tilePalette.Count == 0)
        {
            Debug.LogWarning("Save failed: missing world tile palette to save.");
            return false;
        }

        // Handle invalid save data
        if (worldWidth <= 0 || worldHeight <= 0)
        {
            Debug.LogWarning("Save failed: save width and height must be greater than 0.");
            return false;
        }

        return true;
    }

    private static bool HasValidLoadInputs(string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        if(string.IsNullOrWhiteSpace(saveFileName))
        {
            Debug.LogWarning("Load failed: save file name is empty.");
            return false;
        }

        if (targetTilemap == null)
        {
            Debug.LogWarning("Load failed: missing world tilemap to load tiles onto.");
            return false;
        }

        if (tilePalette == null || tilePalette.Count == 0)
        {
            Debug.LogWarning("Load failed: missing world tile palette to load tiles from.");
            return false;
        }

        return true;
    }

    private static void SaveTerrainTiles(WorldSaveData saveData, int worldWidth, int worldHeight, Tilemap targetTilemap, List<TileBase> tilePalette)
    {
        //  Store the world tilemap as an array of tile palette indexes in world save data
        int offsetX = -(worldWidth / 2);
        int offsetY = -(worldHeight / 2);
        int i = 0;
        for (int y = 0; y < worldHeight; y++)
        {
            for (int x = 0; x < worldWidth; x++)
            {
                // Get position of tile in the world tilemap to store
                Vector3Int tilePosition = new Vector3Int(x + offsetX, y + offsetY, 0);

                // Get the tile from the world tilemap
                TileBase tile = targetTilemap.GetTile(tilePosition);

                // Handle empty tile in the world tilemap
                if (tile == null)
                {
                    saveData.TilePaletteIndices[i] = -1; // Store as -1 index to mean empty
                }
                else
                {
                    // Get the tile index in the tile palette
                    int tileIndex = tilePalette.IndexOf(tile);

                    // Handle tile not in tile palette
                    if (tileIndex == -1)
                    {
                        Debug.LogWarning($"Save warning: tile at {tilePosition} is not in the save tile palette. Saving as empty.");
                    }

                    // Save the tile index in save data
                    saveData.TilePaletteIndices[i] = tileIndex;
                }
                i++;
            }
        }
    }

    private static void LoadTerrainTiles(WorldSaveData saveData, Tilemap targetTilemap, List<TileBase> tilePalette)
    {
        // Render saved world data to world tilemap
        int flatIndex = 0;
        int offsetX = -(saveData.Width / 2);
        int offsetY = -(saveData.Height / 2);
        for (int y = 0; y < saveData.Height; y++)
        {
            for (int x = 0; x < saveData.Width; x++)
            {
                // Get the palette index
                int paletteIndex = saveData.TilePaletteIndices[flatIndex];

                // Initialize tile as empty
                TileBase tile = null;

                // Handle non-empty tile at palette index
                if (paletteIndex != -1)
                {
                    // Handle out of range tile index
                    if (paletteIndex >= tilePalette.Count || paletteIndex < -1)
                    {
                        Debug.LogWarning($"Load failed: saved tile index {paletteIndex} is out of range.");
                        return;
                    }

                    // Set tile to corresponding tile at palette index
                    tile = tilePalette[paletteIndex];
                }

                // Get position of tile in the world tilemap to store
                Vector3Int position = new Vector3Int(x + offsetX, y + offsetY, 0);

                // Set the tile on the world tilemap
                targetTilemap.SetTile(position, tile);

                flatIndex++;
            }
        }
    }

    private static void SavePlacedFeatures(WorldSaveData saveData, int worldWidth, int worldHeight, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        // Store the world features to save data's placed features array
        List<WorldFeatureSaveData> placedFeatures = new List<WorldFeatureSaveData>();

        // Set range of features to be saved
        float minX = -(worldWidth / 2);
        float maxX = minX + worldWidth;
        float minY = -(worldHeight / 2);
        float maxY = minY + worldHeight;

        // Check each child of features parent
        foreach (Transform child in featuresParent)
        {
            // Ignore nulls
            if (child == null)
            {
                continue;
            }

            // Ignore features out of range
            Vector3 position = child.position; 
            if (position.x < minX || position.x >= maxX || position.y < minY || position.y >= maxY)
            {
                continue;
            }

            // Ignore features without a name/id
            if (string.IsNullOrWhiteSpace(child.name))
            {
                Debug.LogWarning("Save warning: skipped a feature with an empty name.");
                continue;
            }

            // Ignore features not on the feature types list
            if (!HasFeatureType(child.name, featureTypes))
            {
                Debug.LogWarning($"Save warning: skipped feature '{child.name}' because it is not in the feature type list.");
                continue;
            }

            // Add the feature to placed features list
            placedFeatures.Add(new WorldFeatureSaveData
            {
                FeatureID = child.name,
                X = child.position.x,
                Y = child.position.y,
            });
        }

        // Store the placed features as an array to save data
        saveData.PlacedFeatures = placedFeatures.ToArray();
    }

    private static bool HasFeatureType(string featureID, List<FeatureTypeData> featureTypes)
    {
        foreach(FeatureTypeData featureType in featureTypes)
        {
            if (featureType != null && featureType.Id == featureID) { return true; }
        }

        return false;
    }

    private static void LoadPlacedFeatures(WorldSaveData saveData, Transform featuresParent, List<FeatureTypeData> featureTypes)
    {
        // Handle if features are not configured for loading
        if (featuresParent == null || featureTypes == null || featureTypes.Count == 0)
        {
            return;
        }

        // Set range of world features to be cleared
        float minX = -(saveData.Width / 2);
        float maxX = minX + saveData.Width;
        float minY = -(saveData.Height / 2);
        float maxY = minY + saveData.Height;

        // Clear existing features inside the loaded world area
        for (int i = featuresParent.childCount - 1; i >= 0; i--)
        {
            Transform child = featuresParent.GetChild(i);
            Vector3 position = child.position;

            if (position.x >= minX && position.x < maxX && position.y >= minY && position.y < maxY)
            {
                Object.Destroy(child.gameObject);
            }
        }

        // Handle if there are no saved placed features
        if (saveData.PlacedFeatures == null || saveData.PlacedFeatures.Length == 0)
        {
            return;
        }

        // Add the saved placed features to featuresParent
        foreach (WorldFeatureSaveData featureSaved in saveData.PlacedFeatures)
        {
            // Ignore nulls
            if (featureSaved == null)
            {
                continue;
            }

            // Ignore features without a name/id
            if (string.IsNullOrWhiteSpace(featureSaved.FeatureID))
            {
                Debug.LogWarning("Load warning: skipped a feature with an empty name.");
                continue;
            }

            // Ignore features not listed in feature types
            FeatureTypeData featureType = GetFeatureTypeByID(featureSaved.FeatureID, featureTypes);
            if (featureType == null)
            {
                Debug.LogWarning($"Load warning: feature ID '{featureSaved.FeatureID}' was not found in the feature type list.");
                continue;
            }

            // Ignore features without a prefab
            if (featureType.Prefab == null)
            {
                Debug.LogWarning($"Load warning: feature type '{featureSaved.FeatureID}' is missing its prefab.");
                continue;
            }

            // Create the feature as a child of features parent
            Vector3 position = new Vector3(featureSaved.X, featureSaved.Y, featuresParent.position.z);
            GameObject featureObject = Object.Instantiate(featureType.Prefab, position, Quaternion.identity, featuresParent);
            featureObject.name = featureSaved.FeatureID;
        }
    }

    private static FeatureTypeData GetFeatureTypeByID(string featureID, List<FeatureTypeData> featureTypes)
    {
        foreach (FeatureTypeData featureType in featureTypes)
        {
            if (featureType != null && featureType.Id == featureID) { return featureType; }
        }

        return null;
    }

    private static void WriteSaveFile(WorldSaveData saveData, string saveFileName)
    {
        // Store the save data to a JSON file at path
        string jsonData = JsonUtility.ToJson(saveData, true); // Convert save data to JSON format
        string path = GetPath(saveFileName);

        try
        {
            File.WriteAllText(path, jsonData); // Write the JSON data at the path
            Debug.Log($"Save successful: {path}");
        }
        catch (IOException e)
        {
            Debug.LogError($"Save failed (IO): {e.Message}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Save failed (Unexpected): {e.Message}");
        }
    }

    private static WorldSaveData ReadSaveFile(string saveFileName)
    {
        // Get the save data path
        string path = GetPath(saveFileName);

        // Handle file not found
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Load failed: Save file not found: {path}");
            return null;
        }

        // Read the file
        string jsonData;
        try
        {
            jsonData = File.ReadAllText(path); // Read the json file from the path
        }
        catch (IOException e)
        {
            Debug.LogError($"Load failed (IO): {e.Message}");
            return null;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed (Unexpected): {e.Message}");
            return null;
        }

        // Parse JSON data
        try
        {
            return JsonUtility.FromJson<WorldSaveData>(jsonData); // Convert JSON to World Save Data
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed (Invalid JSON): {e.Message}");
            return null;
        }
    }
}
