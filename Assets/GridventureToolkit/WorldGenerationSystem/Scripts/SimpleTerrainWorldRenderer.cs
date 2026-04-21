/*
* SimpleTerrainWorldRenderer.cs
* Gridventure Toolkit - Simple Terrain World Renderer
* Author: Lizzie Perez
* Version: 0.0
*/
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Handles converting generated terrain data into visual Tilemap output.
/// Responsible for positioning and placing tiles based on world data.
/// </summary>
public class SimpleTerrainWorldRenderer : WorldRendererBase
{
    /// <summary>
    /// Creates a new world renderer.
    /// </summary>
    /// <param name="config">The world generation configuration containing dimensions and settings.</param>
    /// <param name="worldTilemap">The Unity Tilemap where tiles will be rendered.</param>
    public SimpleTerrainWorldRenderer(WorldGenerationSystemConfig config, Tilemap worldTilemap) : base(config, worldTilemap)
    {

    }

    /// <summary>
    /// Renders the generated terrain to the Tilemap.
    /// </summary>
    /// <param name="worldTerrain">The generated logical terrain grid to render.</param>
    /// <returns>
    /// True if rendering was successful; false if terrain data is invalid.
    /// </returns>
    public override bool Render(TerrainTypeData[,] worldTerrain)
    {
        int width = Config.Width;
        int height = Config.Height;

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
}
