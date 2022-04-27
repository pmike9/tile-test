using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обрабатываемая плитка. Содержит данные для оценки точек и резки плитки.
/// </summary>
public class ProcessedTile
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    /// <summary>
    /// Список угловых точек по индексам, начиная с левой нижней и далее по часовой стрелке.
    /// </summary>
    public List<ProcessedTilePoint> CornerPoints;

    /// <summary>
    /// Списки точек разреза по индексам рёбер. Обход рёбер по часовой стрелке.
    /// Индексы рёбер: 0 - левое ребро, 1 - верхнее ребро, 2 - правое ребро, 3 - нижнее ребро.
    /// </summary>
    public List<List<ProcessedTilePoint>> CuttedEdgePoints;

    /// <summary>
    /// Результат обработки плитки в виде контура точек с обходом по часовой стрелке. 
    /// </summary>
    public List<Vector2> PathToDisplay;

    public bool IsValid;

    public ProcessedTile(Tile origTile)
    {
        Width = origTile.Width;
        Height = origTile.Height;

        CornerPoints = new List<ProcessedTilePoint>();
        for (int i = 0; i < origTile.CornerPoints.Length; i++)
            CornerPoints.Add(new ProcessedTilePoint(origTile.CornerPoints[i]));

        CuttedEdgePoints = new List<List<ProcessedTilePoint>>();
        for (int i = 0; i < 4; i++)
            CuttedEdgePoints.Add(new List<ProcessedTilePoint>());

        PathToDisplay = null;
        IsValid = true;
    }
}