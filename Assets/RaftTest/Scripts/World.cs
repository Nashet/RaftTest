using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public static BlockType AirBlock { get; private set; }

        // allows static access
        public static World Get { get; private set; }

        // Use this for initialization
        public void Awake()
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

            GenerateMap();
        }

        protected void SetUpLogic()
        {
            Get = this;

            // fill map with empty blocks
            map = new Cell[xSize, ySize, zSize];

            AirBlock = new BlockType("Empty air", true, true, null, 1f, false, false, true,
                isFullBlock: false, material: null, maxLengthWithoutSupport: 0);
            Fill(AirBlock);
        }

        /// <summary>
        /// Get highest block in random x,z point
        /// </summary>        
        public Vector3Int GetRandomSpawnPoint(int AmountOfTries)
        {
            var random = new System.Random();

            for (int attemp = 0; attemp < AmountOfTries; attemp++)
            {
                int x = random.Next(xSize);
                int z = random.Next(zSize);

                for (int y = ySize - 1; y >= 0; y--)
                    if (HasAnyNonAirBlock(x, y, z))
                        return new Vector3Int(x, y, z);
            }
            return default(Vector3Int);
        }

        protected void Fill(BlockType block)
        {
            for (int x = 0; x < xSize; x++)
                for (int y = 0; y < ySize; y++)
                    for (int z = 0; z < zSize; z++)
                    {
                        map[x, y, z].Init(block);
                    }
        }

        /// <summary>
        /// Just places several foundation blocks
        /// </summary>
        protected void GenerateMap()
        {
            var x = xSize / 2;
            var z = zSize / 2;
            int zeroLevel = 0;
            var foundation = GManager.Get.AllPlaceable().ElementAt(5);            

            foreach (var validCoord in map.GetCoordsWithRadius(x, z, 2))
            {
                var block = Block.Instantiate(foundation, BlockType.Side.Top, this);
                block.SetPosition(new Vector3(validCoord.x, zeroLevel, validCoord.y), BlockType.Side.Top);
                block.Place(this);
            }
            
        }

        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>    
        public virtual BlockType GetBlock(int x, int y, int z, BlockType.Side side)
        {
            if (IsCellExists(x, y, z))
                return map[x, y, z].Get(side);
            else
                return null;
        }



        /// <summary>
        /// null means that cell doesn't exist (wrong index)
        /// </summary>    
        public virtual BlockType GetBlock(Vector3Int position, BlockType.Side side)
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
                foreach (BlockType.Side eachSide in Enum.GetValues(typeof(BlockType.Side)))
                {
                    if (map[x, y, z].Get(eachSide) != AirBlock)
                        return true;
                }
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

        internal bool HasAnyNonAirBlockExcept(Vector3Int coords, BlockType.Side exceptThatBlock)
        {
            if (IsCellExists(coords.x, coords.y, coords.z))
            {
                foreach (BlockType.Side eachSide in Enum.GetValues(typeof(BlockType.Side)))
                {
                    var block = map[coords.x, coords.y, coords.z].Get(eachSide);
                    if (block != AirBlock && eachSide != exceptThatBlock)
                        return true;
                }
                return false;
            }
            else
                return false;
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
            float planeHeight = 0f;//-blockSize/2f
            m.vertices = new Vector3[] {
         new Vector3(-width / 2f *blockSize, planeHeight, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, planeHeight, height/ 2f*blockSize),
         new Vector3(width/ 2f*blockSize, planeHeight, -height/ 2f*blockSize),
         new Vector3(-width/ 2f*blockSize, planeHeight, -height/ 2f*blockSize),
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
        public virtual void Add(IPlaceable placeable, BlockType.Side sideSnapping)
        {
            var coords = placeable.GetIntegerCoords();

            if (placeable.IsFullBlock) // fill all sides
            {
                foreach (BlockType.Side eachSide in Enum.GetValues(typeof(BlockType.Side)))
                {
                    map[coords.x, coords.y, coords.z].Place(placeable.Type, eachSide);
                }
            }
            else // fill specific part
                map[coords.x, coords.y, coords.z].Place(placeable.Type, sideSnapping);
        }

        public virtual void Remove(Block selectedObject)
        {
            var coords = GetIntegerCoords(selectedObject.transform.position);
            if (IsCellExists(coords.x, coords.y, coords.z))
            {
                if (selectedObject.Type.IsFullBlock)// fill all sides
                {
                    foreach (BlockType.Side eachSide in Enum.GetValues(typeof(BlockType.Side)))
                    {
                        map[coords.x, coords.y, coords.z].Remove(eachSide);

                    }
                }
                else
                {
                    map[coords.x, coords.y, coords.z].Remove(selectedObject.SideSnapping);

                }
                Destroy(selectedObject.gameObject);
            }
        }

        /// <summary>
        /// Allowing faster app reloading
        /// </summary>
        protected void Update()
        {

            if (Get == null)
                Awake();
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
        /// </summary>
        public static BlockType.Side GetClosestSideXZ(Vector3 lookingPosition, Vector3 blockPlacingPosition)
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

            float smallestDist = Mathf.Min(distToWest, distToEast, distToSouth, distToNorth);

            if (distToEast == smallestDist)
                return BlockType.Side.East;
            else if (distToWest == smallestDist)
                return BlockType.Side.West;
            else if (distToSouth == smallestDist)
                return BlockType.Side.South;
            else //if (distToNorth == smallestDist)
                 // default
                return BlockType.Side.North;
        }



        /// <summary>
        /// returns which side of map is closer to point - north, south, west, east        
        /// </summary>
        public static BlockType.Side GetClosestSideXY(Vector3 lookingPosition, Vector3 blockPlacingPosition)
        {
            // distance to block's side
            float xDifference = lookingPosition.x - blockPlacingPosition.x;
            float yDifference = lookingPosition.y - blockPlacingPosition.y;

            Vector2 point = new Vector2(xDifference, yDifference);

            // find to which border it's closer         
            float distToWest = Mathf.Abs(0f - point.x);
            float distToEast = Mathf.Abs(1f - point.x);
            float distToBottom = Mathf.Abs(0f - point.y);
            float distToTop = Mathf.Abs(1f - point.y);

            float smallestDist = Mathf.Min(distToWest, distToEast, distToBottom, distToTop);

            if (distToEast == smallestDist)
                return BlockType.Side.East;
            else if (distToWest == smallestDist)
                return BlockType.Side.West;
            else if (distToTop == smallestDist)
                return BlockType.Side.Top;
            else //if (distToBottom == smallestDist)
                return BlockType.Side.Bottom;//default
        }
        /// <summary>
        /// returns which side of map is closer to point - north, south, west, east        
        /// </summary>
        public static BlockType.Side GetClosestSideY(Vector3 lookingPosition, Vector3 blockPlacingPosition)
        {
            // distance to block's side            
            float yDifference = lookingPosition.y - blockPlacingPosition.y;

            // find to which border it's closer         
            float distToBottom = Mathf.Abs(0f - yDifference);
            float distToTop = Mathf.Abs(1f - yDifference);

            float smallestDist = Mathf.Min(distToBottom, distToTop);

            if (distToTop == smallestDist)
                return BlockType.Side.Top;
            else //if (distToBottom == smallestDist)
                return BlockType.Side.Bottom;//default
        }
        /// <summary>
        /// returns which side of map is closer to point - north, south, west, east, top, bottom        
        /// </summary>
        public static BlockType.Side GetClosestSideXYZ(Vector3 lookingPosition, Vector3 blockPlacingPosition)
        {
            // distance to block's side
            float xDifference = lookingPosition.x - blockPlacingPosition.x;
            float zDifference = lookingPosition.z - blockPlacingPosition.z;
            float yDifference = lookingPosition.y - blockPlacingPosition.y;

            Vector3 point = new Vector3(xDifference, yDifference, zDifference);

            // find to which border it's closer         
            float distToWest = Mathf.Abs(0f - point.x);
            float distToEast = Mathf.Abs(1f - point.x);
            float distToSouth = Mathf.Abs(0f - point.z);
            float distToNorth = Mathf.Abs(1f - point.z);
            float distToBottom = Mathf.Abs(0f - point.y);
            float distToTop = Mathf.Abs(1f - point.y);

            float smallestDist = Mathf.Min(distToWest, distToEast, distToSouth, distToNorth, distToTop, distToBottom);


            if (distToTop == smallestDist)
                return BlockType.Side.Top;
            else if (distToBottom == smallestDist)
                return BlockType.Side.Bottom;
            else if (distToEast == smallestDist)
                return BlockType.Side.East;
            else if (distToWest == smallestDist)
                return BlockType.Side.West;
            else if (distToSouth == smallestDist)
                return BlockType.Side.South;
            else //if (distToNorth == Mathf.Min(Mathf.Min(Mathf.Min(distToWest, distToEast), distToNorth), distToSouth))
                 // default
                return BlockType.Side.North;
        }

        /// <summary>
        /// Just transfers call to map[,,].GetMapElementsWithRadius 
        /// </summary>        
        public IEnumerable<Cell> GetMapElementsWithRadius(int x, int y, int z, int radius)
        {
            // scan neighbor cells for support            
            return map.GetElementsWithRadius(x, y, z, radius);
        }
       
        public virtual void Restart()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            Awake();          
            //GenerateMap();
        }
    }
}