using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// keeps map
/// </summary>
public class World : MonoBehaviour
{
    [SerializeField] private int xSize, zSize, ySize; // y is a height

    [SerializeField] private Material planeMaterial;

    /// <summary>Minimal block size, default is 1, in Unity units, doesn't work if not 1</summary>
    private const int blockSize = 1;

    /// <summary>
    /// holds data about every cell in world
    /// </summary>
    [SerializeField] private Placeable[,,] map;

    /// <summary>
    /// Empty block
    /// </summary>
    public Placeable AirBlock { get { return airBlock; } }
    private Placeable airBlock;

    // allows static access
    public static World Get { get; private set; }

    
    // Use this for initialization
    void Start()
    {
        Get = this;

        // fill map with empty blocks
        map = new Placeable[xSize, zSize, ySize];

        airBlock = new Placeable(false, null, 1f);
        Fill(airBlock);

        GameObject plane = new GameObject("Plane");
        plane.transform.parent = this.transform;

        // move plane away from 0,0
        plane.transform.position = new Vector3(xSize / 2f - blockSize / 2f, 0f, zSize / 2f - blockSize / 2f);

        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreatePlaneMesh(xSize, zSize);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = planeMaterial;
        MeshCollider collider = plane.AddComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.mesh;
    }
    public void Fill(Placeable block)
    {
        
        for (int x = 0; x < xSize; x++)
            for (int z = 0; z < zSize; z++)
                for (int y = 0; y < ySize; y++)
                    map[x, z, y] = airBlock;
    }

    /// <summary>
    /// null means that cell doesn't exist (wrong index)
    /// </summary>    
    public Placeable GetCell(int x, int z, int y)
    {
        if (IsCellExists(x, z, y))
            return map[x, z, y];
        else
            return null;
    }

    /// <summary>
    /// null means that cell doesn't exist (wrong index)
    /// </summary>   
    public bool IsCellExists(int x, int z, int y)
    {
        if (x < xSize && y < ySize && z < zSize && x >= 0 && y >= 0 && z >= 0)
            return true;
        else
            return false;
    }

    public void PlaceBlock(Placeable block)
    {
        var coords = Placeable.GetIntegerCoords(block.GameObject.transform.position);
        if (IsCellExists(coords.x, coords.z, coords.y))
        {
            Debug.Log("Placed block in (x,z,y)" + coords.x + " " + coords.z + " " + coords.y);
            map[coords.x, coords.z, coords.y] = block;
            var newBlock = Object.Instantiate(block.GameObject);

            newBlock.layer = 0; // placed block wouldn't be ignored by raycast
            newBlock.transform.parent = this.transform;
            newBlock.GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    /// <summary>
    /// Adjust coordinates by 0.5, because block center is 0.5, 0.5
    /// </summary>
    /// <param name="coords"></param>
    /// <returns></returns>
    public static Vector3 AdjustCoords(Vector3 coords)
    {
        Vector3 res = new Vector3(coords.x + World.blockSize / 2f, coords.y + World.blockSize / 2f, coords.z + World.blockSize / 2f);
        return res;
    }    

    private Mesh CreatePlaneMesh(float width, float height)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        m.vertices = new Vector3[] {
         new Vector3(-width / 2f *blockSize, 0.00f, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, 0.00f, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, 0.00f, -height/ 2f*blockSize),
         new Vector3(-width/ 2f*blockSize, 0.00f, -height/ 2f*blockSize),
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
}
