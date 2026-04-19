/*
* WorldRenderer.cs
* Gridventure Toolkit - World Renderer
* Author: Lizzie Perez
* Version: 0.0
*/
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles converting generated terrain data into visual Tilemap output.
/// Responsible for positioning and placing tiles based on world data.
/// </summary>
public class WorldRenderer
{
    private WorldGenerationSystemConfig _config;
    private Tilemap _worldTilemap;

    /// <summary>
    /// Creates a new world renderer.
    /// </summary>
    /// <param name="config">
    /// The world generation configuration containing dimensions and settings.
    /// </param>
    /// <param name="worldTileMap">
    /// The Unity Tilemap where tiles will be rendered.
    /// </param>
    public WorldRenderer(WorldGenerationSystemConfig config, Tilemap worldTilemap)
    {
        _config = config;
        _worldTilemap = worldTilemap;
    }

    /// <summary>
    /// Renders the generated terrain to the Tilemap.
    /// Converts terrain data into tile positions and assigns tiles accordingly.
    /// </summary>
    /// <remarks>
    /// Tiles are centered around the origin using an offset based on world size.
    /// Uses Tilemap.SetTiles() for efficient batch placement.
    /// </remarks>
    /// <returns>
    /// True if rendering was successful; false if terrain data is invalid.
    /// </returns>
    public bool Render(TerrainTypeData[,] worldTerrain)
    {
        // Check that config settings match worldTerrain data
        int width = _config.Width;
        int height = _config.Height;
        if ((worldTerrain == null) || (worldTerrain.GetLength(0) != width) || (worldTerrain.GetLength(1) != height))
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

        // Clear the tilemap
        _worldTilemap.ClearAllTiles();

        // Set the tiles on the tilemap
        _worldTilemap.SetTiles(positions, terrainArray);

        return true;
    }
}
