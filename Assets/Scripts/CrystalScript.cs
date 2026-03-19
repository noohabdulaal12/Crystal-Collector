using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CrystalScript : MonoBehaviour
{
    public Color crystalColor = Color.cyan;
    void Start()
    {
        Mesh mesh = new Mesh(); // Making a 3D Shape (Crystal) with triangles
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material.color = crystalColor;

        Vector3[] trianglePoints = new Vector3[] {
            new(0, 3, 0), // Point at the top
            new(3, 0, 0), // Four points in the middle
            new(-3, 0, 0),  
            new(0, 0, 3),
            new(0, 0, -3), 
            new(0, -3, 0)  // Point at the bottom 
        };

        int[] triangles = new int[] {

            // Logically we need the top point four times and the bottom point four times
            // Each middle points connects to the two middle points with +- on the other axis
            // They need to be in specific orders to make sure the triangle is forwards not backwards 
            // I first wrote the points, then checked using math and javascript and made order adjustments
            // The result is like two pyramids placed together to look like a crystal

            0, 3, 1,
            0, 2, 3, 
            0, 4, 2,
            0, 1, 4,
            5, 1, 3, 
            5, 3, 2, 
            5, 2, 4,
            5, 4, 1  
    };

        mesh.vertices = trianglePoints;
        mesh.triangles = triangles;

        mesh.RecalculateNormals(); 
    }
}