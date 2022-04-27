using UnityEngine;

public class CuttedPointResult
{
    public bool IsCuttedPointExist;
    public Vector2 CuttedPoint;
}

/// <summary>
/// Нож на плоскости OxOy, разделяющий её на области. Задаёт правила "резки" плоскости и критерий принадлежности точек задаваемой области.
/// </summary>
public abstract class AreaCutterBase
{
    public abstract bool IsPointInCuttedArea(Vector2 point);

    public abstract CuttedPointResult GetCuttedPointY(float x);

    public abstract CuttedPointResult GetCuttedPointX(float y);

    public CuttedPointResult GetCuttedPointYinRange(float x, float bottomY, float topY)
    {
        var cutPointResult = GetCuttedPointY(x);
        float y = cutPointResult.CuttedPoint.y;
        if (cutPointResult.IsCuttedPointExist)
            if (y > bottomY && y < topY)
                return new CuttedPointResult { CuttedPoint = cutPointResult.CuttedPoint, IsCuttedPointExist = true };

        return new CuttedPointResult { CuttedPoint = Vector2.zero, IsCuttedPointExist = false };
    }

    public CuttedPointResult GetCuttedPointXinRange(float y, float leftX, float rightX)
    {
        var cutPointResult = GetCuttedPointX(y);
        float x = cutPointResult.CuttedPoint.x;
        if (cutPointResult.IsCuttedPointExist)
            if (x > leftX && x < rightX)
                return new CuttedPointResult { CuttedPoint = cutPointResult.CuttedPoint, IsCuttedPointExist = true };

        return new CuttedPointResult { CuttedPoint = Vector2.zero, IsCuttedPointExist = false };
    }
}