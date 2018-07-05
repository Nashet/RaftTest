using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Represents world, keeps map
    /// </summary>
    public class World : MonoBehaviour
    {
        [SerializeField] private int xSize, ySize, zSize; // y is a height

        [SerializeField] private Material planeMaterial;

        /// <summary> Minimal block size, default is 1, in Unity units, doesn't work if not 1</summary>
        private const int blockSize = 1;

        /// <summary>
        /// holds data about every cell in world
        /// </summary>
        [SerializeField] private Cell[,,] map;

        /// <summary>
        /// Empty block
        /// </summary>
        public static Placeable AirBlock { get; private set; }

        // allows static access
        public static World Get { get; private set; }

        // Use this for initialization
        void Start()
        {
            Get = this;

            // fill map with empty blocks
            map = new Cell[xSize, ySize, zSize];

            AirBlock = new Placeable("Empty air", true, null, 1f, false, false, true, isFullBlock: false, material: null);
            Fill(AirBlock);

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
        void Fill(Placeable block)
        {

            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    for (int z = 0; z < zSize; z++)
                    {
                        map[x, y, z].Init(block);
                    }
        }

        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>    
        public Placeable GetBlock(int x, int y, int z, Vector2Int side)
        {
            if (IsCellExists(x, y, z))
                return map[x, y, z].Get(side);
            else
                return null;
        }
        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>    
        public Placeable GetBlock(Vector3Int position, Vector2Int side)
        {
            return GetBlock(position.x, position.y, position.z, side);
        }

        /// <summary>
        /// false also could mean that cell doesn't exist (wrong index)
        /// </summary>    
        public bool HasAnyNonAirBlock(int x, int y, int z)
        {
            if (IsCellExists(x, y, z))
            {
                if (map[x, y, z].Get(Vector2Int.down) != AirBlock
                    || map[x, y, z].Get(Vector2Int.right) != AirBlock
                    || map[x, y, z].Get(Vector2Int.up) != AirBlock
                    || map[x, y, z].Get(Vector2Int.left) != AirBlock
                    || map[x, y, z].Get(Vector2Int.zero) != AirBlock
                    )
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
        /// <summary>
        /// false also could mean that cell doesn't exist (wrong index)
        /// </summary>    
        public bool HasAnyNonAirBlock(Vector3Int position)
        {
            return HasAnyNonAirBlock(position.x, position.y, position.z);
        }

        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>   
        public bool IsCellExists(int x, int y, int z)
        {
            if (x < xSize && y < ySize && z < zSize && x >= 0 && y >= 0 && z >= 0)
                return true;
            else
                return false;
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

        private static Mesh CreatePlaneMesh(float width, float height)
        {
            Mesh m = new Mesh();
            m.name = "ScriptedMesh";
            m.vertices = new Vector3[] {
         new Vector3(-width / 2f *blockSize, 0.00f, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, 0.00f, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, 0.00f, -height/ 2f*blockSize),
         new Vector3(-width/ 2f*blockSize, 0.00f, -height/ 2f*blockSize),
     };
            m.uv = new[] {
         new Vector2 (0, 0),
         new Vector2 (0, 1),
         new Vector2(1, 1),
         new Vector2 (1, 0),
     };
            m.triangles = new int[] { 0, 1, 2, 0, 2, 3 };
            m.RecalculateNormals();

            return m;
        }

        /// <summary>
        /// Coordinates check should be outside
        /// </summary>    
        internal void Add(int x, int y, int z, Placeable placeable, Vector2Int sideSnapping)
        {
            if (placeable.IsFullBlock) // fill all places
            {
                map[x, y, z].Place(placeable, Vector2Int.zero);
                map[x, y, z].Place(placeable, Vector2Int.left);
                map[x, y, z].Place(placeable, Vector2Int.right);
                map[x, y, z].Place(placeable, Vector2Int.up);
                map[x, y, z].Place(placeable, Vector2Int.down);
            }
            else // fill specific part
                map[x, y, z].Place(placeable, sideSnapping);
        }
        /// <summary>
        /// Allowing faster app reloading
        /// </summary>
        void Update()
        {

            if (Get == null)
                Start();
        }
    }
}