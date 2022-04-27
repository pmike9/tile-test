using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Операции, изменяющие обрабатываемую в методах плитку. Используются при применении трафарета к заготовке с плитками.
/// </summary>
public static class TileOperations
{
    /// <summary>
    /// Попытка вставки внутренней точки разреза в плитку.
    /// </summary>
    public static bool TryInsertInnerPoint(ProcessedTile tile, Vector2 point, int validPointRating)
    {
        if (point.x > tile.CornerPoints[0].Point.x && point.x < tile.CornerPoints[3].Point.x &&
            point.y > tile.CornerPoints[0].Point.y && point.y < tile.CornerPoints[1].Point.y)
        {
            // список индексов рёбер, которые принадлежат плитке целиком или содержат валидные точки разреза
            List<int> usedEdgesIndicies = new List<int>();

            for (int i = 0; i < 3; i++)
                if (tile.CornerPoints[i].Rating == validPointRating && tile.CornerPoints[i + 1].Rating == validPointRating)
                    usedEdgesIndicies.Add(i);

            if (tile.CornerPoints[0].Rating == validPointRating && tile.CornerPoints[3].Rating == validPointRating)
                usedEdgesIndicies.Add(3);

            for (int i = 0; i < 4; i++)
                if (tile.CuttedEdgePoints[i].Any(p => p.Rating == validPointRating) && !usedEdgesIndicies.Contains(i))
                    usedEdgesIndicies.Add(i);
            usedEdgesIndicies.Sort();

            // анализ полученных рёбер и вставка точки в контур для возможности его обхода по часовой стрелке
            switch (usedEdgesIndicies.Count)
            {
                case 1:
                    tile.PathToDisplay.Add(point);
                    return true;

                case 2:
                    if (usedEdgesIndicies[0] == 0 && usedEdgesIndicies[1] == 3)
                        tile.PathToDisplay.Insert(2, point);
                    else
                        tile.PathToDisplay.Add(point);
                    return true;

                case 3:
                    if (usedEdgesIndicies[0] == 0 && usedEdgesIndicies[1] == 1 && usedEdgesIndicies[2] == 3)
                    {
                        tile.PathToDisplay.Insert(3, point);
                        return true;
                    }
                    if (usedEdgesIndicies[0] == 0 && usedEdgesIndicies[1] == 2 && usedEdgesIndicies[2] == 3)
                    {
                        tile.PathToDisplay.Insert(2, point);
                        return true;
                    }
                    tile.PathToDisplay.Add(point);
                    return true;

                case 4:
                    int index = tile.CornerPoints.FindIndex(p => p.Rating != validPointRating);
                    if (index > 0)
                        tile.PathToDisplay.Insert(index + 1, point);
                    else
                        tile.PathToDisplay.Insert(0, point);
                    return true;
            }            
        }
        return false;
    }

    /// <summary>
    /// Валидации плитки с первоначальным формированием точек контура до вставки внутренних точек. 
    /// </summary>
    public static void ValidateTile(ProcessedTile tile, int validPointRating)
    {
        var pathToDisplay = new List<Vector2>();
        for (int i = 0; i < 4; i++)
        {
            if (tile.CornerPoints[i].Rating == validPointRating)
                pathToDisplay.Add(tile.CornerPoints[i].Point);

            for (int j = 0; j < tile.CuttedEdgePoints[i].Count; j++)
                if (tile.CuttedEdgePoints[i][j].Rating == validPointRating)
                    pathToDisplay.Add(tile.CuttedEdgePoints[i][j].Point);
        }
        tile.IsValid = pathToDisplay.Count > 0;
        tile.PathToDisplay = pathToDisplay;
    }

    /// <summary>
    /// Окончательная валидация плитки. Плитки с "касательным" контуром, содержащим 2 точки не являются валидными.
    /// </summary>
    public static void ValidateTilePathToDisplay(ProcessedTile tile)
    {
        tile.IsValid = tile.PathToDisplay.Count > 2;
    }

    /// <summary>
    /// Присвоение рейтингов точкам плиток.
    /// </summary>
    public static void SetRatingToTilePoints(ProcessedTile tile, List<AreaCutterBase> cutters)
    {
        // в циклах рейтинг точки плитки увеличивается на 1 при её попадании в область, вырезанную заданным ножом
        foreach (var cutter in cutters)
        {
            foreach (var tilePoint in tile.CornerPoints)
                if (cutter.IsPointInCuttedArea(tilePoint.Point))
                    tilePoint.Rating++;

            foreach (var lst in tile.CuttedEdgePoints)
                foreach (var tilePoint in lst)
                    if (cutter.IsPointInCuttedArea(tilePoint.Point))
                        tilePoint.Rating++;
        }
    }

    /// <summary>
    /// Разрез плитки заданными ножами. Формирует упорядоченные списки точек разреза по рёбрам плитки.
    /// </summary>
    public static void CutTile(ProcessedTile tile, List<AreaCutterBase> cutters)
    {
        foreach (var cutter in cutters)
        {
            ProcessedTilePoint procTilePoint = null;

            procTilePoint = GetCuttedTilePointOnVerticalEdge(cutter, tile.CornerPoints[0], tile.CornerPoints[1]);
            if (procTilePoint != null)
                tile.CuttedEdgePoints[0].Add(procTilePoint);

            procTilePoint = GetCuttedTilePointOnHorizontalEdge(cutter, tile.CornerPoints[1], tile.CornerPoints[2]);
            if (procTilePoint != null)
                tile.CuttedEdgePoints[1].Add(procTilePoint);

            procTilePoint = GetCuttedTilePointOnVerticalEdge(cutter, tile.CornerPoints[3], tile.CornerPoints[2]);
            if (procTilePoint != null)
                tile.CuttedEdgePoints[2].Add(procTilePoint);

            procTilePoint = GetCuttedTilePointOnHorizontalEdge(cutter, tile.CornerPoints[0], tile.CornerPoints[3]);
            if (procTilePoint != null)
                tile.CuttedEdgePoints[3].Add(procTilePoint);
        }

        SortTileCuttedEdgePoints(tile);
    }

    static ProcessedTilePoint GetCuttedTilePointOnVerticalEdge(AreaCutterBase cutter,
        ProcessedTilePoint bottomTilePoint, ProcessedTilePoint topTilePoint)
    {
        var result = cutter.GetCuttedPointYinRange(bottomTilePoint.Point.x,
            bottomTilePoint.Point.y, topTilePoint.Point.y);
        if (result.IsCuttedPointExist)
            return new ProcessedTilePoint(result.CuttedPoint);

        return null;
    }

    static ProcessedTilePoint GetCuttedTilePointOnHorizontalEdge(AreaCutterBase cutter,
        ProcessedTilePoint leftTilePoint, ProcessedTilePoint rightTilePoint)
    {
        var result = cutter.GetCuttedPointXinRange(leftTilePoint.Point.y,
            leftTilePoint.Point.x, rightTilePoint.Point.x);
        if (result.IsCuttedPointExist)
            return new ProcessedTilePoint(result.CuttedPoint);

        return null;
    }

    /// <summary>
    /// Сортировка точек разреза на гранях плитки для дальнейшего их обхода по часовой стрелке.
    /// </summary>
    static void SortTileCuttedEdgePoints(ProcessedTile tile)
    {
        tile.CuttedEdgePoints[0] = tile.CuttedEdgePoints[0].OrderBy(p => p.Point.y).ToList<ProcessedTilePoint>();
        tile.CuttedEdgePoints[1] = tile.CuttedEdgePoints[1].OrderBy(p => p.Point.x).ToList<ProcessedTilePoint>();
        tile.CuttedEdgePoints[2] = tile.CuttedEdgePoints[2].OrderByDescending(p => p.Point.y).ToList<ProcessedTilePoint>();
        tile.CuttedEdgePoints[3] = tile.CuttedEdgePoints[3].OrderByDescending(p => p.Point.x).ToList<ProcessedTilePoint>();
    }
}