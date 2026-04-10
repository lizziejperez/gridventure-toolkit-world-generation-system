# World Generation System — Notes & Concepts

<!-- **Script**: -->

## Plan

### System Workflow
1. Generate map data
2. Place tiles using Tilemap
3. Use Rule Tiles to handle automatic terrain visuals

## Idea

- Use byte tile IDs for minimal storage
- Separate generation logic from visual rendering
- Keep tile data flexible for future expansion

### Structure
- `TileType` enum for tile categories (e.g. grass, water, path)
- `TileData` struct for grouped tile information

## Resources

- https://docs.unity3d.com/Packages/com.unity.2d.tilemap.extras@4.3/manual/RuleTile.html?q=rule