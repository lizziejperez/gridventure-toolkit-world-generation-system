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
    private WorldGenerationSystemConfig config;
    private Tilemap worldTileMap;
    private WorldBuilder worldBuilder;

    /// <summary>
    /// Creates a new world renderer.
    /// </summary>
    /// <param name="config">
    /// The world generation configuration containing dimensions and settings.
    /// </param>
    /// <param name="worldTileMap">
    /// The Unity Tilemap where tiles will be rendered.
    /// </param>
    /// <param name="worldBuilder">
    /// The world builder containing generated terrain data.
    /// </param>
    public WorldRenderer(WorldGenerationSystemConfig config, Tilemap worldTileMap, WorldBuilder worldBuilder)
    {
        this.config = config;
        this.worldTileMap = worldTileMap;
        this.worldBuilder = worldBuilder;
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
    public bool Render()
    {
        // Get the terrain tiles as an array
        TileBase[] terrainArray = worldBuilder.GetWorldTerrain();

        // Check that config settings match
        int width = config.width;
        int height = config.height;
        if (terrainArray.Length != (width * height))
        {
            return false;
        }

        // Set the positions for the tiles on the tilemap
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
        worldTileMap.ClearAllTiles();

        // Set the tiles on the tilemap
        worldTileMap.SetTiles(positions, terrainArray);

        return true;
    }
}
