using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static float GetLineY(Vector2 point1, Vector2 point2, float x)
    {
        return point1.y + (x - point1.x) * (point2.y - point1.y) / (point2.x - point1.x);
    }

    public static float GetLineX(Vector2 point1, Vector2 point2, float y)
    {
        return point1.x + (y - point1.y) * (point2.x - point1.x) / (point2.y - point1.y);
    }

    public static bool Closely(float a, float b)
    {
        return a == b || Mathf.Approximately(a, b);
    }

    public static float MetersToMillimeters(float meter)
    {
        return meter * 1000f;
    }

    public static float MillimetersToMeters(float millimeters)
    {
        return millimeters / 1000f;
    }

    public static float DegreesToRadians(float degrees)
    {       
        return Mathf.Deg2Rad * degrees;
    }

    public static float RadiansToDegrees(float radians)
    {
        return Mathf.Rad2Deg * radians;
    }

    public static float GetTriangleSquare(Vector2 a, Vector2 b, Vector2 c)
    {
        return Mathf.Abs(0.5f * ((a.x - c.x) * (b.y - c.y) - (b.x - c.x) * (a.y - c.y)));
    }

    public static void MeshCombine(GameObject go, Material material)
    {
        var mf = go.AddComponent<MeshFilter>();
        var mr = go.AddComponent<MeshRenderer>();

        MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
            i++;
        }
        mf.mesh = new Mesh();
        mf.mesh.CombineMeshes(combine);
        go.transform.gameObject.SetActive(true);

        mr.material = material;
    }

    public static void DrawPathInDebug(List<Vector2> pathPoints, Color color)
    {
        for (int i = 0; i < pathPoints.Count - 1; i++)
            Debug.DrawLine(pathPoints[i], pathPoints[i + 1], color, 100000000000f);
        Debug.DrawLine(pathPoints[0], pathPoints[pathPoints.Count - 1], color, 100000000000f);
    }

    public static void DrawRaysFrom0InDebug(List<Vector2> pathPoints, Color color)
    {
        for (int i = 1; i < pathPoints.Count; i++)
            Debug.DrawLine(pathPoints[0], pathPoints[i], color, 100000000000f);
    }
}