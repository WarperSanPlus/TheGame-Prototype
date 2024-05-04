using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class OceanWave : MonoBehaviour
{
    private Mesh mesh;

    [SerializeField]
    private Vector2 planeSize = Vector2.one;

    [SerializeField, Min(0.1f)]
    private float resolution = 1;

    // Start is called before the first frame update
    void Start()
    {
        this.mesh = new Mesh() {
            name = "OceanWave Mesh"
        };
        var meshFilter = this.GetComponent<MeshFilter>();
        meshFilter.mesh = this.mesh;

        var scaledSize = new Vector2Int(
            (int)(this.planeSize.x / this.resolution),
            (int)(this.planeSize.y / this.resolution)
        );

        var vertices = GenerateVertices(scaledSize, this.resolution, new Vector3(
            -scaledSize.x / 2, 
            0, 
            -scaledSize.y / 2
        ));
        var triangles = GenerateTriangles(scaledSize);
        this.AssignMesh(vertices, triangles);
    }

    private static int[] GenerateTriangles(Vector2Int size) {

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

    private static Vector3[] GenerateVertices(Vector2Int size, float resolution, Vector3? offset = null) {

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

    private void AssignMesh(Vector3[] vertices, int[] triangles)
    {
        this.mesh.Clear();
        this.mesh.vertices = vertices;
        this.mesh.triangles = triangles;
        this.mesh.RecalculateNormals();
    }
}
