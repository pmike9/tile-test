using UnityEngine;

public class PointRotator
{
    float cosAngle, sinAngle;
    Vector2 centerPoint;

    public Vector2 RotatePoint(Vector2 point)
    {
        return new Vector2(
            centerPoint.x + (point.x - centerPoint.x) * cosAngle - (point.y - centerPoint.y) * sinAngle,
            centerPoint.y + (point.x - centerPoint.x) * sinAngle + (point.y - centerPoint.y) * cosAngle
            );
    }

    public PointRotator(Vector2 centerPoint, float angle)
    {
        this.centerPoint = centerPoint;
        cosAngle = Mathf.Cos(angle);
        sinAngle = Mathf.Sin(angle);
    }
}