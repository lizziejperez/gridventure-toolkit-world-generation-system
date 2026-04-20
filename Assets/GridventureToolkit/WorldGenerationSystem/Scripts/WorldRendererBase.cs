/*
* WorldRendererBase.cs
* Gridventure Toolkit - World Renderer Base
* Author: Lizzie Perez
* Version: 0.0
*/
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Base class for world renderers that convert generated terrain data into visual Tilemap output.
/// This class stores shared renderer dependencies such as the world generation configuration and the target Tilemap. Concrete renderer implementations
/// are responsible for deciding how logical terrain data should be translated into placed tiles.
/// </summary>
public abstract class WorldRendererBase
{
    /// <summary>
    /// Shared world generation configuration used by the renderer.
    /// </summary>
    protected WorldGenerationSystemConfig Config;

    /// <summary>
    /// Tilemap where the world will be rendered.
    /// </summary>
    protected Tilemap WorldTilemap;

    /// <summary>
    /// Renders the generated terrain data to the target Tilemap.
    /// </summary>
    /// <param name="config">The world generation configuration containing dimensions and settings.</param>
    /// <param name="worldTilemap">The Unity Tilemap where tiles will be rendered.</param>
    protected WorldRendererBase(WorldGenerationSystemConfig config, Tilemap worldTilemap) 
    {
        Config = config;
        WorldTilemap = worldTilemap;
    }

    public abstract bool Render(TerrainTypeData[,] worldTerrain);

    /// <summary>
    /// Validates that the provided terrain grid matches the configured world size.
    /// </summary>
    /// <param name="worldTerrain">The terrain grid to validate.</param>
    /// <returns>
    /// True if the terrain grid is non-null and matches the configured width and height; otherwise, false.
    /// </returns>
    protected bool HasValidTerrainDimensions(TerrainTypeData[,] worldTerrain)
    {
        return worldTerrain != null && worldTerrain.GetLength(0) == Config.Width && worldTerrain.GetLength(1) == Config.Height;
    }

    /// <summary>
    /// Calculates centered Tilemap positions for a rectangular region.
    /// </summary>
    /// <param name="width">The width of the rendered region.</param>
    /// <param name="height">The height of the rendered region.</param>
    /// <returns>An array of Tilemap cell positions centered around the origin.</returns>
    protected Vector3Int[] GetCenteredPositions(int width, int height)
    {
        Vector3Int[] positions = new Vector3Int[width * height];
        int offsetX = -(width / 2);
        int offsetY = -(height / 2);
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = y * width + x;
                positions[i] = new Vector3Int(x + offsetX, y + offsetY, 0);
            }
        }

        return positions;
    }

    /// <summary>
    /// Clears all tiles from the target world Tilemap.
    /// </summary>
    protected void ClearTilemap()
    {
        WorldTilemap.ClearAllTiles();
    }

    /// <summary>
    /// Places a batch of tiles on the target world Tilemap.
    /// </summary>
    /// <param name="positions">The Tilemap cell positions where tiles should be placed.</param>
    /// <param name="tiles">The tiles to place at the corresponding positions.</param>
    protected void SetTiles(Vector3Int[] positions, TileBase[] tiles)
    {
        WorldTilemap.SetTiles(positions, tiles);
    }
}
