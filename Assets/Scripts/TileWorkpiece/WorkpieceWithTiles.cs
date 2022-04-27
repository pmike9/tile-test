using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Квадратная область-заготовка, содержащая разложенные по заданным параметрам целые плитки. 
/// Плитки могут выступать за границы области на 1 штуку (запас для вращения и дальнейшей резки).
/// </summary>
public class WorkpieceWithTiles
{
    List<TileStrip> tileStrips;

    public WorkpieceWithTiles(SquareBounds bounds, float tileWidth, float tileHeight, float tileOffset, float tileGap)
    {
        int stripeIndexMin = Mathf.RoundToInt(bounds.Bottom / (tileHeight + tileGap)) - 1;
        int stripeIndexMax = Mathf.RoundToInt(bounds.Top / (tileHeight + tileGap)) + 1;

        tileStrips = new List<TileStrip>();
        for (int i = stripeIndexMin; i <= stripeIndexMax; i++)
            tileStrips.Add(new TileStrip(i, bounds.Left, bounds.Right, tileWidth, tileHeight, tileOffset, tileGap));
    }

    public List<List<ProcessedTile>> ExportToProcessedTiles()
    {
        var procTiles = new List<List<ProcessedTile>>();
        foreach (var strip in tileStrips)
            procTiles.Add(strip.ExportToProcessedTiles());
        return procTiles;
    }
}