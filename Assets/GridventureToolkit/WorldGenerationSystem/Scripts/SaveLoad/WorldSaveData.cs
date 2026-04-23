/*
* WorldSaveData.cs
* Gridventure Toolkit - World Save Data
* Author: Lizzie Perez
* Version: 1.0
*/
using System;

/// <summary>
/// Serializable runtime save data for a generated world.
/// Stores the world dimensions and the flattened tile palette indices in row-major order so the world can be reconstructed later.
/// </summary>
[Serializable]
public class WorldSaveData
{
    /// <summary>
    /// The number of columns in the saved region.
    /// </summary>
    public int Width;

    /// <summary>
    /// The number of rows in the saved region.
    /// </summary>
    public int Height;

    /// <summary>
    /// Flattened tile palette indices in row-major order.
    /// A value of -1 represents an empty cell.
    /// </summary>
    public int[] TilePaletteIndices;

    /// <summary>
    /// The collection of saved feature instances placed in the world.
    /// </summary>
    public WorldFeatureSaveData[] PlacedFeatures;
}
