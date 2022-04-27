using UnityEngine;

/// <summary>
///  Фабрика ножей на плоскости OxOy. Создаваемые ножи режут плоскость по прямой линии и выбирают одну из частей разреза.
/// </summary>
public static class StraightLineCutterFactory
{
    /// <summary>
    /// Метод, создающий нож.
    /// </summary>
    /// <param name="point1">Первая точка, через которую проходит нож.</param>
    /// <param name="point2">Вторая точка, через которую проходит нож.</param>
    /// <param name="areaCheckPoint">Проверочная точка, которая должна принадлежать выбираемой области разреза.</param>
    /// <returns></returns>
    public static AreaCutterBase CreateAreaCutter(Vector2 point1, Vector2 point2, Vector2 areaCheckPoint)
    {
        if (Utils.Closely(point1.x, point2.x))
            return new VerticalAreaCutter(point1.x, areaCheckPoint.x <= point1.x);

        if (Utils.Closely(point1.y, point2.y))
            return new HorizontalAreaCutter(point1.y, areaCheckPoint.y <= point1.y);

        return new LinearAreaCutter(point1, point2, areaCheckPoint.y <= Utils.GetLineY(point1, point2, areaCheckPoint.x));
    }
}