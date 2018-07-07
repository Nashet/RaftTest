using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RaftTest
{
    /// <summary>
    /// Represents block which can be placed in world and can be hold in hands
    /// </summary>
    [Serializable]
    public class Placeable : Nameable, IHideable, IHoldable
    {
        [Tooltip("Turn on if you want to turn of physics for that block and/or include manual collision detection")]
        [SerializeField] private bool isTrigger;

        [Tooltip("Works only for full blocks")]
        [SerializeField] private int maxLengthWithoutSupport;

        [SerializeField] private bool canBePlacedAtZeroLevelWithoutFoundation;

        [SerializeField] private bool isFullBlock;
        /// <summary> Full block means that it fills entire cell, like 1x1, not a wall like 0.5x1     
        public bool IsFullBlock { get { return isFullBlock; } }

        [SerializeField] private bool requiresSomeFoundation;
        [SerializeField] private bool allowsEdgePlacing;
        public bool OnlyCenterPlacing { get { return !allowsEdgePlacing; } }


        [Tooltip("Should be about same as gameObject thickness")]
        [SerializeField] private float blockThickness;

        [SerializeField] private MeshRenderer renderer;

        [SerializeField] private GameObject gameObject;

        /// <summary> Original material    
        [SerializeField] private Material material;

        /// <summary> which side of map it is closer - north, south, west, east. 0,0 if it's center (default)
        private Vector2Int sideSnapping;

        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;
        public static event EventHandler<EventArgs> Placed;
        /// <summary>
        /// Constructor. Instead, you can set values in inspector
        /// </summary>   

        public Placeable(string name, bool allowsEdgePlacing, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material)
        {
            this.name = name;
            this.isTrigger = isTrigger;
            this.requiresSomeFoundation = requiresSomeFoundation;
            this.canBePlacedAtZeroLevelWithoutFoundation = canBePlacedAtZeroLevelWithoutFoundation;
            this.isFullBlock = isFullBlock;
            this.material = material;
            this.name = name;

            this.gameObject = prefab;
            this.allowsEdgePlacing = allowsEdgePlacing;
            if (gameObject != null)
                renderer = gameObject.GetComponent<MeshRenderer>();
            this.blockThickness = blockThickness;
        }

        static Vector3Int GetIntegerCoords(Vector3 position)
        {
            Vector3 adjustedCoords = World.AdjustCoords(position);

            int x = Mathf.FloorToInt(adjustedCoords.x);
            int y = Mathf.FloorToInt(adjustedCoords.y);
            int z = Mathf.FloorToInt(adjustedCoords.z);


            return new Vector3Int(x, y, z);
        }
        /// <summary>
        /// Restores original material, instead of green "allowing" material
        /// </summary>
        void SetOriginalMaterial()
        {
            renderer.material = material;
        }

        /// <summary>
        /// returns which side of map is closer to point - north, south, west, east
        /// 4 sides are coded in following format:
        /// (-1,0),(0,-1),(1,0),(0,1)
        /// </summary>
        static Vector2Int GetClosestSide(Vector3 lookingPosition, Vector3 blockPlacingPosition)
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

        void UpdateMaterial()
        {
            if (CanBePlaced(World.Get))
            {
                this.renderer.material = GManager.Get.BuildingAlowedMaterial; // originalMat;
            }
            else
            {
                this.renderer.material = GManager.Get.BuildingDeniedMaterial;
            }
        }

        /// <summary>
        /// updates block held by player - rotates, changes color if building is not allowed, etc
        /// </summary>
        public void UpdateBlock()
        {
            if (this != null)
            {
                RaycastHit hit;

                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    Vector3 lookingPosition = World.AdjustCoords(hit.point);
                    Vector3 blockPlacingPosition;



                    blockPlacingPosition = GetIntegerCoords(hit.point);

                    // allow block to sticks to 1 of 4 side of a cell
                    if (this.allowsEdgePlacing)
                    {
                        sideSnapping = GetClosestSide(lookingPosition, blockPlacingPosition);

                        blockPlacingPosition.x += sideSnapping.x * (0.5f - this.blockThickness / 2f);
                        blockPlacingPosition.z += sideSnapping.y * (0.5f - this.blockThickness / 2f);
                        if (sideSnapping.y == 0) // rotate block if it's closer to y side
                            this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                        else
                            this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

                    }
                    this.gameObject.transform.position = blockPlacingPosition;
                    Debug.Log("Looking at (x,y,z)" + lookingPosition + " side is " + sideSnapping);
                    //if (EventSystem.current.IsPointerOverGameObject())
                    //    return null;// -3; //hovering over UI
                    // updates holding block color 
                    this.UpdateMaterial();
                }
            }
        }
        bool CanBePlaced(World world)
        {

            var blockPlacementCoords = GetIntegerCoords(this.gameObject.transform.position);
            var placeToBuild = world.GetBlock(blockPlacementCoords, sideSnapping);

            if (placeToBuild == null)
                return false; // wrong index
            else
            {
                if (placeToBuild == World.AirBlock && !placeToBuild.IsFullBlock) // is empty space                                                                             
                {
                    // here go all kinds of foundation checks
                    if (!requiresSomeFoundation)
                        return true;

                    if (canBePlacedAtZeroLevelWithoutFoundation && blockPlacementCoords.y == 0)
                        return true;

                    var bottomBlockCoords = blockPlacementCoords + Vector3Int.down;

                    // checks for neighbor cells
                    if (this.IsFullBlock)
                    {
                        if (!world.HasAnyNonAirBlock(blockPlacementCoords)// any block below, no any blocks here                        
                            && (world.HasAnyNonAirBlock(bottomBlockCoords) || HasHorizontalSupport()))
                            return true;
                        else
                            return false;
                    }
                    else // not full block //can be center part or one of 4 edge parts
                    {
                        var blockBelowThatHalfBlock = world.GetBlock(bottomBlockCoords, sideSnapping);
                        if (blockBelowThatHalfBlock != null && blockBelowThatHalfBlock.IsFullBlock //full block below
                           || blockBelowThatHalfBlock != null && blockBelowThatHalfBlock != World.AirBlock) // there is half block below in right position        
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
        }

        private bool HasHorizontalSupport()
        {
            // scan neighbor cells for support
            var pos = GetIntegerCoords(gameObject.transform.position);
            int xStart = pos.x - maxLengthWithoutSupport;
            int xEnd = pos.x + maxLengthWithoutSupport;
            int zStart = pos.z - maxLengthWithoutSupport;
            int zEnd = pos.z + maxLengthWithoutSupport;
            int level = pos.y - 1; // search in lower level
            for (int i = xStart; i <= xEnd; i++)
                for (int j = zStart; j <= zEnd; j++)
                    if (World.Get.HasAnyNonAirBlock(i, level, j))
                        return true;
            return false;
        }

        /// <summary>
        /// Creates copy of this object ready to put in a world
        /// </summary>    
        GameObject Instantiate()
        {
            SetOriginalMaterial();
            var newBlock = UnityEngine.Object.Instantiate(this.gameObject);
            newBlock.layer = 0; // placed block wouldn't be ignored by raycast

            if (this.isTrigger)
                newBlock.GetComponent<Collider>().isTrigger = true;
            else
                newBlock.GetComponent<Collider>().isTrigger = false;
            return newBlock;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            EventHandler<EventArgs> handler = Shown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArgs> handler = Hidden;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void PlaceBlock(World world)
        {
            if (this.CanBePlaced(world))
            {
                var coords = Placeable.GetIntegerCoords(this.gameObject.transform.position);

                var newBlock = this.Instantiate();
                newBlock.transform.parent = world.transform;
                Debug.Log("Placed block in (x,y,z)" + coords + " with snapping " + sideSnapping);
                world.Add(coords.x, coords.y, coords.z, this, sideSnapping);

                EventHandler<EventArgs> handler = Placed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }
    }
}