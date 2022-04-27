using UnityEngine;

public class Tile
{
    public float Width { get; private set; }
    public float Height { get; private set; }

    /// <summary>
    /// Список угловых точек по индексам, начиная с левой нижней и далее по часовой стрелке.
    /// </summary>

    public Vector2[] CornerPoints { get; private set; }

    public Tile(float width, float height, Vector2 leftBottomPoint)
    {
        Width = width;
        Height = height;

        CornerPoints = new Vector2[4];
        CornerPoints[0] = leftBottomPoint;
        CornerPoints[1] = new Vector2(leftBottomPoint.x, leftBottomPoint.y + Height);
        CornerPoints[2] = new Vector2(leftBottomPoint.x + Width, leftBottomPoint.y + Height);
        CornerPoints[3] = new Vector2(leftBottomPoint.x + Width, leftBottomPoint.y);
    }
}