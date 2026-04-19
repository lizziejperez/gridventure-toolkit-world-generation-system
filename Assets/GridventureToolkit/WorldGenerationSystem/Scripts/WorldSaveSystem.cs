/*
* WorldSaveSystem.cs
* Gridventure Toolkit - World Save System
* Author: Lizzie Perez
* Version: 0.0
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
    /// <param name="saveOrigin">The bottom-left Tilemap cell position where the saved region begins.</param>
    /// <param name="width">The number of columns to save.</param>
    /// <param name="height">The number of rows to save.</param>
    /// <param name="fileName">The save file name without extension.</param>
    /// <param name="worldTilemap">The Tilemap to read tiles from.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert TileBase references into saved indices.</param>
    /// <remarks>
    /// Tiles are stored in row-major order using the index formula: index = y * width + x.
    /// A value of -1 is stored for empty cells.
    /// </remarks>
    public static void Save(Vector3Int saveOrigin, int width, int height, string fileName, Tilemap worldTilemap, List<TileBase> tilePalette)
    {
        // Handle missing save data
        if (worldTilemap == null)
        {
            Debug.LogWarning("Save failed: missing world tilemap to save.");
            return;
        }
        if (tilePalette == null || tilePalette.Count == 0)
        {
            Debug.LogWarning("Save failed: missing world tile palette to save.");
            return;
        }

        // Handle invalid save data
        if (width <= 0 || height <= 0)
        {
            Debug.LogWarning("Save failed: save width and height must be greater than 0.");
            return;
        }

        // Initilize the save data
        WorldSaveData saveData = new WorldSaveData
        {
            OriginX = saveOrigin.x,
            OriginY = saveOrigin.y,
            Width = width,
            Height = height,
            WorldTileIndexes = new int[width * height]
        };

        //  Store the world tilemap as thier tile palette indexes in world save data
        int i = 0;
        for (int offsetY = 0; offsetY < height; offsetY++)
        {
            for (int offsetX = 0; offsetX < width; offsetX++)
            {
                // Get position of tile in the world tilemap to store
                Vector3Int position = new Vector3Int(saveOrigin.x + offsetX, saveOrigin.y + offsetY, 0);

                // Get the tile from the world tilemap
                TileBase tile = worldTilemap.GetTile(position);

                // Handle empty tile in the world tilemap
                if (tile == null)
                {
                    saveData.WorldTileIndexes[i] = -1; // Store as -1 index
                }
                else
                {
                    // Get the tile index in the tile palette
                    int tileIndex = tilePalette.IndexOf(tile);

                    // Handle tile not in tile palette
                    if (tileIndex == -1)
                    {
                        Debug.LogWarning($"Save failed: tile at {position} is not in the save tile palette.");
                        return;
                    }

                   // Save the tile index in save data
                    saveData.WorldTileIndexes[i] = tileIndex;
                }
                i++;
            }
        }

        // Store the save data to a JSON file at path
        string jsonData = JsonUtility.ToJson(saveData, true); // Convert save data to JSON format
        string path = GetPath(fileName);
        File.WriteAllText(path, jsonData); // Write the JSON data at the path
    }

    /// <summary>
    /// Loads a saved Tilemap region from a JSON file and applies it to the provided Tilemap.
    /// </summary>
    /// <param name="fileName">The save file name without extension.</param>
    /// <param name="worldTilemap">The Tilemap to populate with saved tiles.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert saved indices back into TileBase references.</param>
    /// <remarks>
    /// Saved tiles are restored starting at the saved origin stored in the save file.
    /// A saved value of -1 clears the corresponding Tilemap cell.
    /// </remarks>
    public static void Load(string fileName, Tilemap worldTilemap, List<TileBase> tilePalette)
    {
        // Handle missing expected inputs
        if (worldTilemap == null)
        {
            Debug.LogWarning("Load failed: missing world tilemap to load tiles onto.");
            return;
        }
        if (tilePalette == null || tilePalette.Count == 0)
        {
            Debug.LogWarning("Load failed: missing world tile palette to load tiles from.");
            return;
        }

        string path = GetPath(fileName);

        // Handle file not found
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Load failed: Save file not found: {path}");
            return;
        }

        string jsonData = File.ReadAllText(path); // Read the json file from the path
        WorldSaveData saveData = JsonUtility.FromJson<WorldSaveData>(jsonData); // Convert JSON to World Save Data

        // Handle null data
        if (saveData == null)
        {
            Debug.LogWarning("Load failed: save data is null.");
            return;
        }
        if (saveData.WorldTileIndexes == null)
        {
            Debug.LogWarning("Load failed: saved tile index array is missing.");
            return;
        }

        // Handle unexpected data
        if (saveData.WorldTileIndexes.Length != (saveData.Width * saveData.Height))
        {
            Debug.LogWarning("Load failed: saved tile count dows not match saved width and height.");
            return;
        }

        // Render saved world data to world tilemap
        int i = 0;
        for (int offsetY = 0; offsetY < saveData.Height; offsetY++)
        {
            for (int offsetX = 0; offsetX < saveData.Width; offsetX++)
            {
                // Get the tile index
                int tileIndex = saveData.WorldTileIndexes[i];

                // Initialite tile as empty
                TileBase tile = null;

                // Handle non-empty tile at tile index
                if (tileIndex != -1)
                {
                    // Handle out of range tile index
                    if (tileIndex >= tilePalette.Count || tileIndex < -1)
                    {
                        Debug.LogWarning($"Load failed: saved tile index {tileIndex} is out of range.");
                        return;
                    }

                    // Set tile to corresponding tilePallete tile at tile index
                    tile = tilePalette[tileIndex];                    
                }

                // Get position of tile in the world tilemap to store
                Vector3Int position = new Vector3Int(saveData.OriginX + offsetX, saveData.OriginY + offsetY, 0);

                // Set the tile on the world tilemap
                worldTilemap.SetTile(position, tile);

                i++;
            }
        }
    }

    /// <summary>
    /// Builds the full save file path for a world save JSON file.
    /// </summary>
    /// <param name="fileName">The save file name without extension.</param>
    /// <returns>
    /// The absolute path to the save file in Application.persistentDataPath.
    /// </returns>
    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }
}
