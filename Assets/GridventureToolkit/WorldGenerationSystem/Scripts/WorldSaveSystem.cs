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
    /// <param name="regionOrigin">The bottom-left Tilemap cell position where the saved region begins.</param>
    /// <param name="regionWidth">The number of columns to save.</param>
    /// <param name="regionHeight">The number of rows to save.</param>
    /// <param name="saveFileName">The save file name without extension.</param>
    /// <param name="targetTilemap">The Tilemap to read tiles from.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert TileBase references into saved indices.</param>
    /// <remarks>
    /// Tiles are stored in row-major order using the index formula: index = y * width + x.
    /// A value of -1 is stored for empty cells.
    /// </remarks>
    public static void Save(Vector3Int regionOrigin, int regionWidth, int regionHeight, string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette)
    {
        // Handle missing save data
        if (targetTilemap == null)
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
        if (regionWidth <= 0 || regionHeight <= 0)
        {
            Debug.LogWarning("Save failed: save width and height must be greater than 0.");
            return;
        }

        // Initialize the save data
        WorldSaveData saveData = new WorldSaveData
        {
            OriginX = regionOrigin.x,
            OriginY = regionOrigin.y,
            Width = regionWidth,
            Height = regionHeight,
            TilePaletteIndices = new int[regionWidth * regionHeight]
        };

        //  Store the world tilemap as an array of tile palette indexes in world save data
        int i = 0;
        for (int offsetY = 0; offsetY < regionHeight; offsetY++)
        {
            for (int offsetX = 0; offsetX < regionWidth; offsetX++)
            {
                // Get position of tile in the world tilemap to store
                Vector3Int tilePosition = new Vector3Int(regionOrigin.x + offsetX, regionOrigin.y + offsetY, 0);

                // Get the tile from the world tilemap
                TileBase tile = targetTilemap.GetTile(tilePosition);

                // Handle empty tile in the world tilemap
                if (tile == null)
                {
                    saveData.TilePaletteIndices[i] = -1; // Store as -1 index
                }
                else
                {
                    // Get the tile index in the tile palette
                    int tileIndex = tilePalette.IndexOf(tile);

                    // Handle tile not in tile palette
                    if (tileIndex == -1)
                    {
                        Debug.LogWarning($"Save failed: tile at {tilePosition} is not in the save tile palette.");
                        return;
                    }

                   // Save the tile index in save data
                    saveData.TilePaletteIndices[i] = tileIndex;
                }
                i++;
            }
        }

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

    /// <summary>
    /// Loads a saved Tilemap region from a JSON file and applies it to the provided Tilemap.
    /// </summary>
    /// <param name="saveFileName">The save file name without extension.</param>
    /// <param name="targetTilemap">The Tilemap to populate with saved tiles.</param>
    /// <param name="tilePalette">The ordered list of supported tiles used to convert saved indices back into TileBase references.</param>
    /// <remarks>
    /// Saved tiles are restored starting at the saved origin stored in the save file.
    /// A saved value of -1 clears the corresponding Tilemap cell.
    /// </remarks>
    public static void Load(string saveFileName, Tilemap targetTilemap, List<TileBase> tilePalette)
    {
        // Handle missing expected inputs
        if (targetTilemap == null)
        {
            Debug.LogWarning("Load failed: missing world tilemap to load tiles onto.");
            return;
        }
        if (tilePalette == null || tilePalette.Count == 0)
        {
            Debug.LogWarning("Load failed: missing world tile palette to load tiles from.");
            return;
        }

        string path = GetPath(saveFileName);

        // Handle file not found
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Load failed: Save file not found: {path}");
            return;
        }

        string jsonData;
        try
        {
            jsonData = File.ReadAllText(path); // Read the json file from the path
        }
        catch (IOException e) 
        {
            Debug.LogError($"Load failed (IO): {e.Message}");
            return;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed (Unexpected): {e.Message}");
            return;
        }

        WorldSaveData saveData;
        try
        {
            saveData = JsonUtility.FromJson<WorldSaveData>(jsonData); // Convert JSON to World Save Data
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Load failed (Invalid JSON): {e.Message}");
            return;
        }

        // Handle null data
        if (saveData == null)
        {
            Debug.LogWarning("Load failed: save data is null.");
            return;
        }
        if (saveData.TilePaletteIndices == null)
        {
            Debug.LogWarning("Load failed: saved tile index array is missing.");
            return;
        }

        // Handle unexpected data
        if (saveData.TilePaletteIndices.Length != (saveData.Width * saveData.Height))
        {
            Debug.LogWarning("Load failed: saved tile count does not match saved width and height.");
            return;
        }

        // Render saved world data to world tilemap
        int flatIndex = 0;
        for (int offsetY = 0; offsetY < saveData.Height; offsetY++)
        {
            for (int offsetX = 0; offsetX < saveData.Width; offsetX++)
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
                Vector3Int position = new Vector3Int(saveData.OriginX + offsetX, saveData.OriginY + offsetY, 0);

                // Set the tile on the world tilemap
                targetTilemap.SetTile(position, tile);

                flatIndex++;
            }
        }
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
}
