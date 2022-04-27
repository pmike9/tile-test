using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Форма с плитками после резки заготовки по шаблону.
/// Служит для рендера и расчётов с обработанными плитками. 
/// </summary>
public class ShapeWithTiles
{
    List<List<ProcessedTile>> procTiles;
    Vector2 rotationCenter;

    public ShapeWithTiles(List<List<ProcessedTile>> procTiles, Vector2 rotationCenter)
    {
        this.procTiles = procTiles;
        this.rotationCenter = rotationCenter;
    }

    /// <summary>
    /// Рендер формы и получение видимой площади плиток.
    /// </summary>
    /// <param name="parentGo">Родительский объект-контейнер плиток.</param>
    /// <param name="tileMaterial">Материал плиток.</param>
    /// <param name="rotationAngle">Угол поворота формы относительно центра вращения.</param>
    /// <returns>Площадь видимой части плиток в кв.м.</returns>
    public float DrawShapeGetTilesSquare(ref GameObject parentGo, Material tileMaterial, float rotationAngle)
    {
        parentGo = new GameObject("ShapeWithTiles");
        float visibleSquare = 0;

        foreach (var lst in procTiles)
            foreach (var tile in lst)
                if (tile.IsValid && tile.PathToDisplay.Count > 2)
                    visibleSquare += DrawTileGetVisibleSquare(parentGo, tile, tileMaterial);

        //Utils.MeshCombine(parentGo, tileMaterial);

        parentGo.transform.RotateAround(rotationCenter, Vector3.forward, Utils.RadiansToDegrees(rotationAngle));

        return visibleSquare;
    }

    float DrawTileGetVisibleSquare(GameObject parentGo, ProcessedTile tile, Material tileMaterial)
    {
        float square = 0;

        Vector3[] vertices = tile.PathToDisplay.ConvertAll<Vector3>(p => new Vector3(p.x, p.y, 0)).ToArray();
        int[] triangles = new int[3 * (vertices.Length - 2)];
        Vector3[] normals = new Vector3[vertices.Length];

        for (int i = 0; i < normals.Length; i++)
            normals[i] = -Vector3.forward;

        for (int i = 0; i < vertices.Length - 2; i++)
        {
            triangles[3 * i] = 0;
            triangles[3 * i + 1] = i + 1;
            triangles[3 * i + 2] = i + 2;

            square += Utils.GetTriangleSquare(vertices[0], vertices[i + 1], vertices[i + 2]);
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(
                (vertices[i].x - tile.CornerPoints[0].Point.x) / tile.Width,
                (vertices[i].y - tile.CornerPoints[0].Point.y) / tile.Height);
        }

        var mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;

        var go = new GameObject("Mesh");
        var mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        var mr = go.AddComponent<MeshRenderer>();
        mr.material = tileMaterial;

        go.transform.parent = parentGo.transform;

        return square;
    }
}