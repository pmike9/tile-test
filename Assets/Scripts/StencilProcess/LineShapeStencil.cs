using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Трафарет, применяемый для резки квадратной заготовки с плитками. 
/// Трафарет строится на основе задаваемой связной области и угла её поворота.
/// </summary>
public class LineShapeStencil
{
    List<Vector2> vertexPoints;

    List<AreaCutterBase> areaCutters;
    int validTilePointRating;
    Vector2 rotationCenter;

    public LineShapeStencil(ConnectedLineRegion lineRegion, float rotationAngle)
    {
        rotationCenter = lineRegion.RotationCenter;
        if (Utils.Closely(rotationAngle, 0) || rotationAngle == 0)
            vertexPoints = lineRegion.VertexPoints;
        else
        {
            vertexPoints = new List<Vector2>();
            var rotator = new PointRotator(rotationCenter, rotationAngle);
            foreach (var point in lineRegion.VertexPoints)
                vertexPoints.Add(rotator.RotatePoint(point));
        }

        areaCutters = new List<AreaCutterBase>();
        var pCount = vertexPoints.Count;
        for (int i = 0; i < pCount - 2; i++)
            areaCutters.Add(
                StraightLineCutterFactory.CreateAreaCutter(vertexPoints[i], vertexPoints[i + 1], vertexPoints[i + 2]));
        areaCutters.Add(
            StraightLineCutterFactory.CreateAreaCutter(vertexPoints[pCount - 2], vertexPoints[pCount - 1], vertexPoints[0]));
        areaCutters.Add(
            StraightLineCutterFactory.CreateAreaCutter(vertexPoints[0], vertexPoints[pCount - 1], vertexPoints[1]));

        validTilePointRating = areaCutters.Count;
    }

    /// <summary>
    /// Применение трафарета к заданной заготовке с плитками. 
    /// Производится резка плиток и возвращается полученная форма с обработанными плитками.
    /// </summary>
    public ShapeWithTiles Apply(WorkpieceWithTiles workpiece)
    {
        var procTiles = workpiece.ExportToProcessedTiles();

        foreach (var lst in procTiles)
            foreach (var tile in lst)
            {
                TileOperations.CutTile(tile, areaCutters);

                TileOperations.SetRatingToTilePoints(tile, areaCutters);

                TileOperations.ValidateTile(tile, validTilePointRating);

                if (tile.IsValid)
                    foreach (var point in vertexPoints)
                        if (TileOperations.TryInsertInnerPoint(tile, point, validTilePointRating))
                            break;

                TileOperations.ValidateTilePathToDisplay(tile);
            }

        return new ShapeWithTiles(procTiles, rotationCenter);
    }
}
