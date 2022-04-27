using UnityEngine;

public class HorizontalAreaCutter : AreaCutterBase
{
    float cutterY;
    bool isCuttedAreaInBottomSide;

    public override CuttedPointResult GetCuttedPointX(float y)
    {
        return new CuttedPointResult { CuttedPoint = Vector2.zero, IsCuttedPointExist = false };
    }

    public override CuttedPointResult GetCuttedPointY(float x)
    {
        return new CuttedPointResult { CuttedPoint = new Vector2(x, cutterY), IsCuttedPointExist = true };
    }

    public override bool IsPointInCuttedArea(Vector2 point)
    {
        return isCuttedAreaInBottomSide ? point.y <= cutterY : point.y >= cutterY;
    }

    public HorizontalAreaCutter(float cutterY, bool isCuttedAreaInBottomSide)
    {
        this.cutterY = cutterY;
        this.isCuttedAreaInBottomSide = isCuttedAreaInBottomSide;
    }
}