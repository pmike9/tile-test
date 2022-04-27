using System.Collections.Generic;
using UnityEngine;

public class TileStrip
{
    List<Tile> tiles;

    public TileStrip(int index, float leftBound, float rightBound, 
        float tileWidth, float tileHeight, float tileOffset, float tileGap)
    {
        int tileIndexMin = Mathf.RoundToInt((leftBound - tileOffset * index) / (tileWidth + tileGap)) - 1;
        int tileIndexMax = Mathf.RoundToInt((rightBound - tileOffset * index) / (tileWidth + tileGap)) + 1;

        float initLeftBottomY = index * (tileHeight + tileGap);
        float x = index * tileOffset;

        tiles = new List<Tile>();
        for (int i = tileIndexMin; i <= tileIndexMax; i++)
            tiles.Add(new Tile(tileWidth, tileHeight, new Vector2(x + (tileWidth + tileGap) * i, initLeftBottomY)));
    }

    public List<ProcessedTile> ExportToProcessedTiles()
    {        
        var procTiles = new List<ProcessedTile>();
        foreach (var tile in tiles)
            procTiles.Add(new ProcessedTile(tile));
        return procTiles;
    }
}