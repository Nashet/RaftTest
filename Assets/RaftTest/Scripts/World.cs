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
        [SerializeField] protected int xSize, ySize, zSize; // y is a height

        [SerializeField] protected Material planeMaterial;

        /// <summary> Minimal block size, default is 1, in Unity units, doesn't work if not 1</summary>
        protected const int blockSize = 1;

        /// <summary>
        /// holds data about every cell in world
        /// </summary>
        [SerializeField] protected Cell[,,] map;

        /// <summary>
        /// Empty block
        /// </summary>
        public static Placeable AirBlock { get; private set; }

        // allows static access
        public static World Get { get; private set; }

        // Use this for initialization
        protected void Start()
        {
            SetUpLogic();
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
        protected void SetUpLogic()
        {
            Get = this;

            // fill map with empty blocks
            map = new Cell[xSize, ySize, zSize];

            AirBlock = new Placeable("Empty air", true, null, 1f, false, false, true,
                isFullBlock: false, material: null, maxLengthWithoutSupport: 0);
            Fill(AirBlock);
        }
        protected void Fill(Placeable block)
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
        public virtual Placeable GetBlock(int x, int y, int z, Vector2Int side)
        {
            if (IsCellExists(x, y, z))
                return map[x, y, z].Get(side);
            else
                return null;
        }

        public virtual void Remove(PlacedBlock selectedObject)
        {
            var coords = GetIntegerCoords(selectedObject.transform.position);
            if (IsCellExists(coords.x, coords.y, coords.z))
            {
                map[coords.x, coords.y, coords.z].Remove(selectedObject.sideSnapping);
                Destroy(selectedObject.gameObject);
            }
        }

        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>    
        public virtual Placeable GetBlock(Vector3Int position, Vector2Int side)
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
        public virtual void Add(Placeable placeable, Vector2Int sideSnapping)
        {
            var coords = placeable.GetIntegerCoords();

            if (placeable.IsFullBlock) // fill all places
            {
                map[coords.x, coords.y, coords.z].Place(placeable, Vector2Int.zero);
                map[coords.x, coords.y, coords.z].Place(placeable, Vector2Int.left);
                map[coords.x, coords.y, coords.z].Place(placeable, Vector2Int.right);
                map[coords.x, coords.y, coords.z].Place(placeable, Vector2Int.up);
                map[coords.x, coords.y, coords.z].Place(placeable, Vector2Int.down);
            }
            else // fill specific part
                map[coords.x, coords.y, coords.z].Place(placeable, sideSnapping);
        }
        /// <summary>
        /// Allowing faster app reloading
        /// </summary>
        protected void Update()
        {

            if (Get == null)
                Start();
        }

        public static Vector3Int GetIntegerCoords(Vector3 position)
        {
            Vector3 adjustedCoords = World.AdjustCoords(position);

            int x = Mathf.FloorToInt(adjustedCoords.x);
            int y = Mathf.FloorToInt(adjustedCoords.y);
            int z = Mathf.FloorToInt(adjustedCoords.z);

            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// returns which side of map is closer to point - north, south, west, east
        /// 4 sides are coded in following format:
        /// (-1,0),(0,-1),(1,0),(0,1)
        /// </summary>
        public static Vector2Int GetClosestSide(Vector3 lookingPosition, Vector3 blockPlacingPosition)
        {
            // distance to block's side
            float xDifference = lookingPosition.x - blockPlacingPosition.x;
            float zDifference = lookingPosition.z - blockPlacingPosition.z;
            Vector2 point = new Vector2(xDifference, zDifference);

            // find to which border it's closer         
            float distToWest = Mathf.Abs(0f - point.x);
            float distToEast = Mathf.Abs(1f - point.x);
            float distToSouth = Mathf.Abs(0f - point.y);
            float distToNorth = Mathf.Abs(1f - point.y);

            if (distToEast == Mathf.Min(Mathf.Min(Mathf.Min(distToWest, distToEast), distToNorth), distToSouth))
                return new Vector2Int(1, 0);
            else if (distToWest == Mathf.Min(Mathf.Min(Mathf.Min(distToWest, distToEast), distToNorth), distToSouth))
                return new Vector2Int(-1, 0);
            else if (distToSouth == Mathf.Min(Mathf.Min(Mathf.Min(distToWest, distToEast), distToNorth), distToSouth))
                return new Vector2Int(0, -1);
            else //if (distToNorth == Mathf.Min(Mathf.Min(Mathf.Min(distToWest, distToEast), distToNorth), distToSouth))
                 // default
                return new Vector2Int(0, 1);
        }
    }
}