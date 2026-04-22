/*
* WorldSaveLoadController.cs
* Gridventure Toolkit - World Save & Load Controller
* Author: Lizzie Perez
* Version: 0.0
*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

/// <summary>
/// Manages saving and loading a configured rectangular Tilemap region through the world save system.
/// </summary>
/// <remarks>
/// This controller stores the file name id, save origin, save dimensions,
/// target Tilemap, and tile palette used for world save and load operations.
/// It can be called from UI buttons or runtime input.
/// </remarks>
[RequireComponent(typeof(PlayerInput))]
public class WorldSaveLoadController : MonoBehaviour
{
    [Header("Save Slot Settings")]
    [SerializeField] private string _saveSlotID = "0";

    [Header("Save Region Settings")]
    [SerializeField] private int _saveRegionWidth = 18;
    [SerializeField] private int _saveRegionHeight = 10;    

    [Header("Terrain Settings")]
    [SerializeField] private Tilemap _targetTilemap;
    [SerializeField] private List<TileBase> _tilePalette;

    [Header("Feature Settings")]
    [SerializeField] private Transform _featuresParent;
    [SerializeField] private List<FeatureTypeData> _featureTypes;

    private PlayerInput saveLoadInput;
    private InputAction saveAction, loadAction;

    private void Awake()
    {
        saveLoadInput = GetComponent<PlayerInput>();
        saveAction = saveLoadInput.actions["Save"];
        loadAction = saveLoadInput.actions["Load"];

        if (saveAction == null)
        {
            Debug.LogWarning("Missing the Save action in player input map.");
        }

        if (loadAction == null)
        {
            Debug.LogWarning("Missing the Load action in player input map.");
        }
    }

    private void OnEnable()
    {
        if (saveAction != null)
        {
            saveAction.started += SaveWorld;
        }

        if (loadAction != null)
        {
            loadAction.started += LoadWorld;
        }        
    }

    private void OnDisable()
    {
        if (saveAction != null)
        {
            saveAction.started -= SaveWorld;
        }

        if (loadAction != null)
        {
            loadAction.started -= LoadWorld;
        }
    }

    /// <summary>
    /// Saves the configured Tilemap region to the configured save file.
    /// </summary>
    private void SaveWorld(InputAction.CallbackContext callbackContext)
    {
        // Handle null or empty save slot ID
        if (string.IsNullOrWhiteSpace(_saveSlotID))
        {
            Debug.LogWarning("Save failed: file name ID is empty.");
            return;
        }

        // Save the Tilemap region to the save file
        WorldSaveSystem.Save(_saveRegionWidth, _saveRegionHeight, BuildSaveFileName(), _targetTilemap, _tilePalette, _featuresParent, _featureTypes);
    }

    /// <summary>
    /// Loads the configured save file and applies its saved region to the Tilemap.
    /// </summary>
    private void LoadWorld(InputAction.CallbackContext callbackContext)
    {
        // Handle null or empty save slot ID
        if (string.IsNullOrWhiteSpace(_saveSlotID))
        {
            Debug.LogWarning("Load failed: file name ID is empty.");
            return;
        }

        // Load saved Tilemap region from the save file
        WorldSaveSystem.Load(BuildSaveFileName(), _targetTilemap, _tilePalette, _featuresParent, _featureTypes);
    }

    /// <summary>
    /// Builds the save file name used by the world save system.
    /// </summary>
    /// <returns>The save file name for the configured save file.</returns>
    private string BuildSaveFileName()
    {
        return "world_save_" + _saveSlotID;
    }
}
