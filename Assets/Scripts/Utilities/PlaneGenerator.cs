using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that generates a plane with the given parameters
/// </summary>
public class PlaneGenerator : MonoBehaviour
{
    [SerializeField, Tooltip("Determines how big the plane is")]
    private Vector2 planeSize = Vector2.one;

    [SerializeField, Min(0.1f), Tooltip("Determines how precised the mesh is")]
    private float resolution = 1;

    /// <inheritdoc/>
    void Start()
    {
        if (!this.TryGetComponent(out MeshFilter meshFilter))
        {
            Debug.LogWarning($"No MeshFilter found in '{this.gameObject.name}'.");
            return;
        }

        meshFilter.mesh = GenerateMesh(this.planeSize, this.resolution);
    }

    /// <summary>
    /// Generates a mesh for a plane with the given size and the given resolution
    /// </summary>
    /// <param name="size">Size of the plane</param>
    /// <param name="resolution">Resolution of the plane</param>
    /// <param name="name">Name of the mesh</param>
    /// <returns>Generated mesh</returns>
    private static Mesh GenerateMesh(Vector2 size, float resolution = 1, string name = null) 
    {
        // Create mesh
        var scaledSize = new Vector2Int(
            (int)(size.x / resolution),
            (int)(size.y / resolution)
        );

        var vertices = GenerateVertices(scaledSize, resolution, new Vector3(
            -scaledSize.x / 2,
            0,
            -scaledSize.y / 2
        ));
        var triangles = GenerateTriangles(scaledSize);

        var mesh = new Mesh()
        {
            name = name ?? "Generated Plane"
        };

        // Assign mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    /// <summary>
    /// Generates the vertices for a plane of the given size and the given resolution
    /// </summary>
    /// <param name="size">Size of the plane</param>
    /// <param name="resolution">How detailed is the plane</param>
    /// <param name="offset">Offset of each vertice</param>
    /// <returns>Generated vertices</returns>
    private static Vector3[] GenerateVertices(Vector2Int size, float resolution, Vector3? offset = null)
    {

        var totalAmount = (size.x + 1) * (size.y + 1);
        var vertices = new Vector3[totalAmount];

        offset ??= Vector3.zero;

        for (var i = 0; i < vertices.Length; i++)
        {
            var y = i / (size.y + 1);
            var x = i % (size.x + 1);

            vertices[i] = (new Vector3(x, 0, y) + offset.Value) * resolution;
        }

        return vertices;
    }

    /// <summary>
    /// Generates the triangles for a plane of the given size
    /// </summary>
    /// <param name="size">Size of the plane</param>
    /// <returns>Generated triangles</returns>
    private static int[] GenerateTriangles(Vector2Int size)
    {

        var triangles = new List<int>();

        for (var y = 0; y < size.y; y++)
        {
            for (var x = 0; x < size.x; x++)
            {
                // Create triangles
                var i = x + ((size.x + 1) * y);
                var sharedPoint = i + size.x + 1;

                // First
                triangles.Add(sharedPoint);
                triangles.Add(i + 1);
                triangles.Add(i);

                // Second
                triangles.Add(i + 1);
                triangles.Add(sharedPoint);
                triangles.Add(sharedPoint + 1);
            }
        }

        return triangles.ToArray();
    }
}