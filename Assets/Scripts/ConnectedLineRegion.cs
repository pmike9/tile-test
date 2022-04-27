using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Связная (выпуклая) область, задаваемая списком точек-вершин, которые образуют контур. 
/// Также задаётся центр и вычисляется квадрат, который полностью покрывает область при любом вращении.
/// </summary>
public class ConnectedLineRegion
{
    public List<Vector2> VertexPoints { get; private set; }

    public Vector2 RotationCenter { get; private set; }

    public ConnectedLineRegion(List<Vector2> vertexPoints, Vector2 rotationCenter)
    {
        VertexPoints = vertexPoints;
        RotationCenter = rotationCenter;
    }

    public SquareBounds GetRotationCoveringBounds()
    {
        float r = 0;
        foreach (var point in VertexPoints)
        {
            float d = Vector2.Distance(RotationCenter, point);
            if (d > r)
                r = d;
        }
        return new SquareBounds(2 * r, RotationCenter);
    }
}
