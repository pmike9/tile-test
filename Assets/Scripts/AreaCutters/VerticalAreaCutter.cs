using UnityEngine;

public class VerticalAreaCutter : AreaCutterBase
{
    float cutterX;
    bool isCuttedAreaInLeftSide;

    public override CuttedPointResult GetCuttedPointX(float y)
    {
        return new CuttedPointResult { CuttedPoint = new Vector2(cutterX, y), IsCuttedPointExist = true };
    }

    public override CuttedPointResult GetCuttedPointY(float x)
    {
        return new CuttedPointResult { CuttedPoint = Vector2.zero, IsCuttedPointExist = false };
    }

    public override bool IsPointInCuttedArea(Vector2 point)
    {
        return isCuttedAreaInLeftSide ? point.x <= cutterX : point.x >= cutterX;
    }

    public VerticalAreaCutter(float cutterX, bool isCuttedAreaInLeftSide)
    {
        this.cutterX = cutterX;
        this.isCuttedAreaInLeftSide = isCuttedAreaInLeftSide;
    }
}