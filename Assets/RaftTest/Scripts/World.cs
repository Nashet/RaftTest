using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// keeps map
/// </summary>
public class World : MonoBehaviour
{

    [SerializeField] private int xSize, zSize, ySize; // y is a height

    [SerializeField] private Material material;

    /// <summary>
    /// holds data about every cell in world
    /// </summary>
    [SerializeField] private Placeable[,,] map;
    
    /// <summary>
    /// Empty block
    /// </summary>
    public Placeable Air { get; private set; }

    // allows static access
    public static World Get { get; private set; }
    // Use this for initialization
    void Start()
    {
       
        Get = this;

        map = new Placeable[xSize, zSize, ySize];
        Air = new Placeable(false, null);
        for (int x = 0; x < xSize; x++)
            for (int z = 0; z < zSize; z++)
                for (int y = 0; y < ySize; y++)
                
                    map[x, z, y] = Air;//Manager.Get.block1; //


        GameObject plane = new GameObject("Plane");
        plane.transform.parent = this.transform;
        MeshFilter meshFilter = (MeshFilter)plane.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = CreateMesh(xSize, zSize);
        MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        renderer.material = material;
        MeshCollider collider = plane.AddComponent<MeshCollider>();
        collider.sharedMesh = meshFilter.mesh;
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
    public bool IsCellExists(int x, int z, int y)
    {
        if (x < xSize && y < ySize && z < zSize && x >= 0 && y >= 0 && z >= 0)
            return true;
        else
            return false;
    }

    public void PlaceBlock(Placeable block)
    {
        var coordinats = block.GetCoordinats();
        if (IsCellExists(coordinats.x, coordinats.z, coordinats.y))
        {
            Debug.Log("Placed block in (x,z,y)" + coordinats.x + " " + coordinats.z + " " + coordinats.y);
            map[coordinats.x, coordinats.z, coordinats.y] = block;
            var newBlock = Object.Instantiate(block.gameObject);

            newBlock.layer = 0; // placed block wouldn't be ignored by raycast
            newBlock.transform.parent = this.transform;
        }
    }
    public bool CanBePlaced(Placeable blockToPlace)
    {
        var coordinats = blockToPlace.GetCoordinats();
        var cell = GetCell(coordinats.x, coordinats.z, coordinats.y);
        if (cell == null)
            return false; // wrong index
        else 
        {
            if (cell == Air)
            {
                if (blockToPlace.allowsMultipleObjectsInCell) // is wall
                {
                    // check if underlying cell exists and not empty
                    var coordsToCheck = coordinats;
                    coordsToCheck.y -= 1;
                    var uderlyingCell = GetCell(coordsToCheck.x, coordsToCheck.z, coordsToCheck.y);
                    if (uderlyingCell == null || uderlyingCell == Air)
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }


            //else if (blockToPlace.allowsMultipleObjectsInCell && cell.allowsMultipleObjectsInCell)
            //    return true;// fix that? can build several walls in single cell
            else
                return false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private Mesh CreateMesh(float width, float height)
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
}
