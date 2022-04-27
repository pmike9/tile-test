using UnityEngine;

public class SquareBounds
{
    public float Left { get; private set; }
    public float Right { get; private set; }
    public float Bottom { get; private set; }
    public float Top { get; private set; }
    public Vector2 Center { get; private set; }

    public SquareBounds(float size, Vector2 center)
    {
        float s2 = size / 2;
        Center = center;
        Left = Center.x - s2;
        Right = Center.x + s2;
        Bottom = Center.y - s2;
        Top = Center.y + s2;
    }
}
