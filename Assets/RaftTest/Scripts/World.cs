using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// keeps map
/// </summary>
public class World : MonoBehaviour {

    [SerializeField] private int xSize;
    [SerializeField] private int ySize;
    [SerializeField] private Material material;
    // Use this for initialization
    void Start () {
        //var size : float;

        GameObject plane = new GameObject("Plane");
        plane.transform.parent = this.transform;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(xSize, ySize);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = material;//.shader = Shader.Find("Particles/Additive");
        MeshCollider collider = plane.AddComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.mesh;       

    }
    Mesh CreateMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-width, 0.01f, height),
         new Vector3(width, 0.01f, height),
         new Vector3(width, 0.01f, -height),
         new Vector3(-width, 0.01f, -height),
     };
        m.uv = new Vector2[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2(1, 1),
         new Vector2 (1, 0)
     };
        m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
        m.RecalculateNormals();

        return m;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
