using UnityEngine;

public class LinearAreaCutter : AreaCutterBase
{
    const float Delta = 0.0001f;

    Vector2 point1;
    float k_xy, k_yx;

    bool isCuttedAreaInBottomSide;

    float LineY(float x)
    {
        return point1.y + k_yx * (x - point1.x);
    }

    float LineX(float y)
    {
        return point1.x + k_xy * (y - point1.y);
    }

    public override CuttedPointResult GetCuttedPointX(float y)
    {
        return new CuttedPointResult { CuttedPoint = new Vector2(LineX(y), y), IsCuttedPointExist = true };
    }

    public override CuttedPointResult GetCuttedPointY(float x)
    {
        return new CuttedPointResult { CuttedPoint = new Vector2(x, LineY(x)), IsCuttedPointExist = true };
    }

    public override bool IsPointInCuttedArea(Vector2 point)
    {
        float y = LineY(point.x);
        return isCuttedAreaInBottomSide ? point.y <= y + Delta : point.y >= y - Delta;
    }

    public LinearAreaCutter(Vector2 point1, Vector2 point2, bool isCuttedAreaInBottomSide)
    {        
        this.isCuttedAreaInBottomSide = isCuttedAreaInBottomSide;
        this.point1 = point1;

        k_yx = (point2.y - point1.y) / (point2.x - point1.x);
        k_xy = 1 / k_yx;
    }
}