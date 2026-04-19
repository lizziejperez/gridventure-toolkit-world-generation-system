/*
* WorldSaveLoadController.cs
* Gridventure Toolkit - World Save & Load Controller
* Author: Lizzie Perez
* Version: 0.0
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages saving and loading a configured rectangular Tilemap region through the world save system.
/// </summary>
/// <remarks>
/// This controller stores the file name id, save origin, save dimensions,
/// target Tilemap, and tile palette used for world save and load operations.
/// It can be called from UI buttons or runtime input.
/// </remarks>
public class WorldSaveLoadController : MonoBehaviour
{
    [Header("World Save & Load Settings")]
    [SerializeField] private string _fileNameId = "0";
    [SerializeField] private Vector3Int _saveOrigin;
    [SerializeField] private int _saveHeight;
    [SerializeField] private int _saveWidth;
    [SerializeField] private Tilemap _worldTilemap;
    [SerializeField] private List<TileBase> _tilePalette;    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Make keyboard input S/s call SaveWorld
        // TODO: Make keyboard input L/l call LoadWorld
    }

    /// <summary>
    /// Saves the configured Tilemap region to the configured save file.
    /// </summary>
    public void SaveWorld()
    {
        // Handle null or empty fileNameId
        if (string.IsNullOrWhiteSpace(_fileNameId))
        {
            Debug.LogWarning("Save failed: file name ID is empty.");
            return;
        }

        // Save the Tilemap region to the save file
        WorldSaveSystem.Save(_saveOrigin, _saveWidth, _saveHeight, "world_save_" + _fileNameId, _worldTilemap, _tilePalette);
    }

    /// <summary>
    /// Loads the configured save file and applies its saved region to the Tilemap.
    /// </summary>
    public void LoadWorld()
    {
        // Handle null or empty fileNameId
        if (string.IsNullOrWhiteSpace(_fileNameId))
        {
            Debug.LogWarning("Load failed: file name ID is empty.");
            return;
        }

        // Load saved Tilemap region from the save file
        WorldSaveSystem.Load("world_save_" + _fileNameId, _worldTilemap, _tilePalette);
    }
}
