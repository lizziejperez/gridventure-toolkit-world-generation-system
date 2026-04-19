/*
* WorldSaveData.cs
* Gridventure Toolkit - World Save Data
* Author: Lizzie Perez
* Version: 0.0
*/
using System;

/// <summary>
/// Serializable runtime save data for a generated world.
/// Stores the world dimensions and the flattened terrain id grid so the world can be reconstructed later.
/// </summary>
[Serializable]
public class WorldSaveData
{
    /// <summary>
    /// The Tilemap cell X coordinate where the saved region begins.
    /// </summary>
    public int OriginX;

    /// <summary>
    /// The Tilemap cell Y coordinate where the saved region begins.
    /// </summary>
    public int OriginY;

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
    public int[] WorldTileIndexes;
}
