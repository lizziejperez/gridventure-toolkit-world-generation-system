/*
* TerrainRenderer.cs
* Gridventure Toolkit - Terrain Renderer
* Author: Lizzie Perez
* Version: 1.0
*/
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles converting generated terrain data into visual Tilemap output.
/// Responsible for positioning and placing tiles based on world data.
/// </summary>
public class TerrainRenderer
{
    /// <summary>
    /// Shared world generation configuration used by the renderer.
    /// </summary>
    private WorldGenerationSystemConfig _config;

    /// <summary>
    /// Tilemap where the world will be rendered.
    /// </summary>
    private Tilemap _terrainTilemap;

    /// <summary>
    /// Renders the generated terrain data to the target Tilemap.
    /// </summary>
    /// <param name="config">The world generation configuration containing dimensions and settings.</param>
    /// <param name="terrainTilemap">The Unity Tilemap where tiles will be rendered.</param>
    public TerrainRenderer(WorldGenerationSystemConfig config, Tilemap terrainTilemap)
    {
        _config = config;
        _terrainTilemap = terrainTilemap;
    }

    /// <summary>
    /// Renders the provided terrain data to the Tilemap using the configured world dimensions.
    /// </summary>
    /// <param name="worldTerrain">The terrain grid to render</param>
    /// <returns>True if the terrain can be rendered; otherwise false.</returns>
    public bool Render(TerrainTypeData[,] worldTerrain)
    {
        int width = _config.Width;
        int height = _config.Height;

        // Check that config settings match worldTerrain data
        if (!HasValidTerrainDimensions(worldTerrain))
        {
            return false;
        }

        // Convert world terrain grid to a corresponding tile array
        TileBase[] terrainArray = new TileBase[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int i = y * width + x;
                terrainArray[i] = worldTerrain[x, y].Tile;
            }
        }

        // Get the positions for the tiles on the tilemap
        Vector3Int[] positions = GetCenteredPositions(width, height);

        // Clear the tilemap
        ClearTilemap();

        // Set the tiles on the tilemap
        SetTiles(positions, terrainArray);

        return true;
    }

    /// <summary>
    /// Validates that the provided terrain grid matches the configured world size.
    /// </summary>
    /// <param name="worldTerrain">The terrain grid to validate.</param>
    /// <returns>
    /// True if the terrain grid is non-null and matches the configured width and height; otherwise, false.
    /// </returns>
    private bool HasValidTerrainDimensions(TerrainTypeData[,] worldTerrain)
    {
        return worldTerrain != null && worldTerrain.GetLength(0) == _config.Width && worldTerrain.GetLength(1) == _config.Height;
    }

    /// <summary>
    /// Calculates centered Tilemap positions for a rectangular region.
    /// </summary>
    /// <param name="width">The width of the rendered region.</param>
    /// <param name="height">The height of the rendered region.</param>
    /// <returns>An array of Tilemap cell positions centered around the origin.</returns>
    private Vector3Int[] GetCenteredPositions(int width, int height)
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
    private void ClearTilemap()
    {
        _terrainTilemap.ClearAllTiles();
    }

    /// <summary>
    /// Places a batch of tiles on the target world Tilemap.
    /// </summary>
    /// <param name="positions">The Tilemap cell positions where tiles should be placed.</param>
    /// <param name="tiles">The tiles to place at the corresponding positions.</param>
    private void SetTiles(Vector3Int[] positions, TileBase[] tiles)
    {
        _terrainTilemap.SetTiles(positions, tiles);
    }
}
