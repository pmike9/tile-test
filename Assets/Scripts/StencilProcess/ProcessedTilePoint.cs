using UnityEngine;

/// <summary>
/// Обрабатываемая точка, принадлежащая плитке.
/// </summary>
public class ProcessedTilePoint
{
    public Vector2 Point;

    /// <summary>
    /// Рейтинг точки для определения её принадлежности области.
    /// </summary>
    public int Rating;

    public ProcessedTilePoint(Vector2 point, int rating = 0)
    {
        Point = point;
        Rating = rating;
    }
}
