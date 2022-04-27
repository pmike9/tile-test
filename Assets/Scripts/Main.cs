using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Material TileMaterial;

    GameObject shapeParentGo;

    float wall_Width = 5000f;
    float wall_Height = 3000f;

    float hexagon_Edge = 2000f;

    float tile_Width = 300f;
    float tile_Height = 100f;
    float tile_Offset = 100f;
    float tile_Gap = 10f;

    float rotationAngel = 60f;

    float tiles_VisibleSquare;

    bool isRectangleWallDemo;

    string wall_Width_str, wall_Height_str, hexagon_Edge_str, tile_Width_str,
        tile_Height_str, tile_Offset_str, tile_Gap_str, rotationAngel_str;

    void Start()
    {        
        wall_Width_str = wall_Width.ToString("0");
        wall_Height_str = wall_Height.ToString("0");
        hexagon_Edge_str = hexagon_Edge.ToString("0");
        tile_Width_str = tile_Width.ToString("0");
        tile_Height_str = tile_Height.ToString("0");
        tile_Offset_str = tile_Offset.ToString("0");
        tile_Gap_str = tile_Gap.ToString("0");
        rotationAngel_str = rotationAngel.ToString("0");

        DrawRectangleWithTiles();
    }

    void GuiStringParameterShow(string nameStr, string unitsStr, string paramStr)
    {
        GUILayout.BeginHorizontal();
        GUI.contentColor = Color.blue;
        GUILayout.Label(nameStr);
        GUILayout.Label(paramStr);
        GUILayout.Label(unitsStr);
        GUILayout.EndHorizontal();
    }

    void GuiStringParameterEnter(string nameStr, string unitsStr, ref string paramStr)
    {
        GUILayout.BeginHorizontal();
        GUI.contentColor = Color.blue;
        GUILayout.Label(nameStr);
        GUI.contentColor = Color.white;
        paramStr = GUILayout.TextField(paramStr, GUILayout.Width(100));
        GUI.contentColor = Color.blue;
        GUILayout.Label(unitsStr);
        GUILayout.EndHorizontal();
    }

    void GuiParameterAction(string nameStr, string unitsStr, ref string paramStr, ref float parameter)
    {
        float temp = 0;
        GuiStringParameterEnter(nameStr, unitsStr, ref paramStr);
        if (float.TryParse(paramStr, out temp))
            if (!Utils.Closely(temp, parameter))
            {
                parameter = temp;
                RedrawDemoShape();
            }
    }

    void OnGUI()
    {
        GUI.contentColor = Color.white;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Прямоугольная стена"))
            DrawRectangleWithTiles();
        if (GUILayout.Button("Шестиугольная стена"))
            DrawHexagonWithTiles();        
        GUILayout.EndHorizontal();

        if (isRectangleWallDemo)
        {
            GuiParameterAction("Ширина стены: ", "мм", ref wall_Width_str, ref wall_Width);
            GuiParameterAction("Высота стены: ", "мм", ref wall_Height_str, ref wall_Height);
        }
        else
            GuiParameterAction("Длина грани: ", "мм", ref hexagon_Edge_str, ref hexagon_Edge);

        GUI.contentColor = Color.grey;
        GUILayout.Label("Параметры плитки и укладки");

        GuiParameterAction("Ширина плитки: ", "мм", ref tile_Width_str, ref tile_Width);
        GuiParameterAction("Высота плитки: ", "мм", ref tile_Height_str, ref tile_Height);
        GuiParameterAction("Смещение плиток: ", "мм", ref tile_Offset_str, ref tile_Offset);
        GuiParameterAction("Шов между плитками: ", "мм", ref tile_Gap_str, ref tile_Gap);
        GuiParameterAction("Угол наклона: ", "гр", ref rotationAngel_str, ref rotationAngel);

        GUI.contentColor = Color.grey;
        GUILayout.Label("Выходные параметры");
        GuiStringParameterShow("Площадь видимой части плиток: ", "кв.м", tiles_VisibleSquare.ToString("0.0"));
    }

    void DrawShapeWithTiles(ConnectedLineRegion shapeRegion)
    {
        DeleteShape();

        var workpiece = new WorkpieceWithTiles(shapeRegion.GetRotationCoveringBounds(),
            Utils.MillimetersToMeters(tile_Width), Utils.MillimetersToMeters(tile_Height),
            Utils.MillimetersToMeters(tile_Offset), Utils.MillimetersToMeters(tile_Gap));

        var stencil = new LineShapeStencil(shapeRegion, Utils.DegreesToRadians(rotationAngel));
        var shape = stencil.Apply(workpiece);

        tiles_VisibleSquare = shape.DrawShapeGetTilesSquare
            (ref shapeParentGo, TileMaterial, -Utils.DegreesToRadians(rotationAngel));
    }

    void DrawRectangleWithTiles()
    {
        isRectangleWallDemo = true;
        float w = Utils.MillimetersToMeters(wall_Width);
        float h = Utils.MillimetersToMeters(wall_Height);
        var region = new ConnectedLineRegion(new List<Vector2>
            {
                 new Vector2(0, 0), new Vector2(0, h),
                 new Vector2(w, h), new Vector2(w, 0),
            },
            new Vector2(w/2, h/2));

        DrawShapeWithTiles(region);
    }

    void DrawHexagonWithTiles()
    {
        isRectangleWallDemo = false;
        Vector2 hexagon_Center = new Vector2(0, 0);

        float edge = Utils.MillimetersToMeters(hexagon_Edge);
        var region = new ConnectedLineRegion(new List<Vector2>
             {
                new Vector2(hexagon_Center.x - edge, hexagon_Center.y),
                new Vector2(hexagon_Center.x - (edge / 2), hexagon_Center.y + (((hexagon_Center.x + edge) * Mathf.Sqrt(3)) / 2)),
                new Vector2(hexagon_Center.x + (edge / 2), hexagon_Center.y + (((hexagon_Center.x + edge) * Mathf.Sqrt(3)) / 2)),
                new Vector2(hexagon_Center.x + edge, hexagon_Center.y),
                new Vector2(hexagon_Center.x + (edge / 2), hexagon_Center.y - (((hexagon_Center.x + edge) * Mathf.Sqrt(3)) / 2)),
                new Vector2(hexagon_Center.x - (edge / 2), hexagon_Center.y - (((hexagon_Center.x + edge) * Mathf.Sqrt(3)) / 2)),
            },
            hexagon_Center);

        DrawShapeWithTiles(region);
    }

    void DeleteShape()
    {
        if (shapeParentGo != null)
        {
            GameObject.Destroy(shapeParentGo);
            shapeParentGo = null;
        }
    }

    void RedrawDemoShape()
    {
        if (isRectangleWallDemo)
            DrawRectangleWithTiles();
        else
            DrawHexagonWithTiles();
    }
}