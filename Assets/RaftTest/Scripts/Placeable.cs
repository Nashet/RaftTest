using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RaftTest
{
    /// <summary>
    /// Represents block which can be placed in world and can be hold in hands    
    /// </summary>
    [Serializable]
    public class Placeable : Nameable, IPlaceable
    {
        [Tooltip("Turn on if you want to turn of physics for that block and/or include manual collision detection")]
        [SerializeField] private bool isTrigger;

        [SerializeField] private int maxLengthWithoutSupport;

        [SerializeField] private bool canBePlacedAtZeroLevelWithoutFoundation;

        [SerializeField] private bool isFullBlock;
        /// <summary> Full block means that it fills entire cell, like 1x1, not a wall like 0.5x1     
        public bool IsFullBlock { get { return isFullBlock; } }

        [SerializeField] private bool requiresSomeFoundation;

        [SerializeField] private bool allowsXZSnapping;
        [SerializeField] private bool allowsYSnapping;
        public bool OnlyCenterPlacing { get { return !allowsXZSnapping && !allowsYSnapping; } }


        [Tooltip("Should be about same as gameObject thickness")]
        [SerializeField] private float blockThickness;

        protected MeshRenderer renderer;

        protected GameObject block;

        [SerializeField] protected GameObject prefab;

        /// <summary> Original material    
        private Material originalMaterial;

        /// <summary> which side of map it is closer - north, south, west, east. default is center
        protected Side sideSnapping;

        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;
        public static event EventHandler<EventArgs> Placed;

        public enum Side { Center, West, East, North, South, Top, Bottom }
        /// <summary>
        /// Constructor. Instead, you can set values in inspector
        /// </summary>   
        public Placeable(string name, bool allowsXZSnapping, bool allowsYSnapping, GameObject prefab, float blockThickness, bool isTrigger, bool requiresSomeFoundation, bool canBePlacedAtZeroLevelWithoutFoundation, bool isFullBlock, Material material, int maxLengthWithoutSupport)
        {
            this.name = name;
            this.isTrigger = isTrigger;
            this.requiresSomeFoundation = requiresSomeFoundation;
            this.canBePlacedAtZeroLevelWithoutFoundation = canBePlacedAtZeroLevelWithoutFoundation;
            this.isFullBlock = isFullBlock;
            this.originalMaterial = material;
            this.name = name;

            this.block = prefab;
            this.allowsXZSnapping = allowsXZSnapping;
            this.allowsYSnapping = allowsYSnapping;
            if (block != null)
                renderer = block.GetComponent<MeshRenderer>();
            this.blockThickness = blockThickness;
        }

        /// <summary>
        /// Used for manual positioning. Alternatively block can be self positioned in UpdateBlock()
        /// </summary>        
        public void SetPosition(Vector3 position, Side side)
        {
            sideSnapping = side;
            position = AdjustSnappingXZ(position);
            position = AdjustSnappingY(position);
            AdjustSnappingRotation();
            block.transform.position = position;
        }

        public Vector3Int GetIntegerCoords()
        {
            Vector3 adjustedCoords = World.AdjustCoords(block.transform.position);

            int x = Mathf.FloorToInt(adjustedCoords.x);
            int y = Mathf.FloorToInt(adjustedCoords.y);
            int z = Mathf.FloorToInt(adjustedCoords.z);


            return new Vector3Int(x, y, z);
        }

        protected void UpdateMaterial()
        {
            if (CanBePlaced(World.Get))
            {
                SetMaterial(GManager.Get.BuildingAlowedMaterial);
            }
            else
            {
                SetMaterial(GManager.Get.BuildingDeniedMaterial);
            }
        }

        /// <summary>
        /// updates block held by player - rotates, changes color if building is not allowed, etc
        /// </summary>
        public virtual void UpdateBlock()
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit))
            {
                Vector3 lookingPosition = World.AdjustCoords(hit.point);
                Vector3 blockPlacingPosition = World.GetIntegerCoords(hit.point);

                // allow block to sticks to 1 of 4 side of a cell
                if (allowsXZSnapping && allowsYSnapping)
                {
                    sideSnapping = World.GetClosestSideXYZ(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingXZ(blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingY(blockPlacingPosition);
                }
                else if (allowsXZSnapping)
                {
                    sideSnapping = World.GetClosestSideXZ(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingXZ(blockPlacingPosition);
                }
                else if (allowsYSnapping)
                {
                    sideSnapping = World.GetClosestSideY(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingY(blockPlacingPosition);
                }


                AdjustSnappingRotation();


                this.block.transform.position = blockPlacingPosition;
                Debug.Log("Looking at (x,y,z)" + lookingPosition + " side is " + sideSnapping);
               
                // updates holding block color 
                this.UpdateMaterial();
            }
        }
        protected Vector3 AdjustSnappingY(Vector3 blockPlacingPosition)
        {
            if (sideSnapping == Side.Top)
                blockPlacingPosition.y += (0.5f - this.blockThickness / 2f);
            else if (sideSnapping == Side.Bottom)
                blockPlacingPosition.y -= (0.5f - this.blockThickness / 2f);
            return blockPlacingPosition;
        }
        protected Vector3 AdjustSnappingXZ(Vector3 blockPlacingPosition)
        {
            if (sideSnapping == Side.East)
                blockPlacingPosition.x += (0.5f - this.blockThickness / 2f);
            else if (sideSnapping == Side.West)
                blockPlacingPosition.x -= (0.5f - this.blockThickness / 2f);
            else if (sideSnapping == Side.North)
                blockPlacingPosition.z += (0.5f - this.blockThickness / 2f);
            else if (sideSnapping == Side.South)
                blockPlacingPosition.z -= (0.5f - this.blockThickness / 2f);
            return blockPlacingPosition;
        }
        protected void AdjustSnappingRotation()
        {
            if (sideSnapping == Side.North || sideSnapping == Side.South) // rotate block at Y axis depending on what side is closer
                this.block.transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            if (sideSnapping == Side.East || sideSnapping == Side.West) // rotate block at Y axis depending on what side is closer
                this.block.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            if (sideSnapping == Side.Top || sideSnapping == Side.Bottom) // rotate block at Z axis depending on what side is closer
                this.block.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        protected bool HasVerticalSupport(World world)
        {
            var bottomBlockCoords = World.GetIntegerCoords(this.block.transform.position) + Vector3Int.down;
            if (IsFullBlock)
            {
                if (world.HasAnyNonAirBlockExcept(bottomBlockCoords, Placeable.Side.Bottom))
                    return true;
                else
                    return false;
            }
            else
            {
                var blockBelowThatHalfBlock = world.GetBlock(bottomBlockCoords, sideSnapping);
                if (blockBelowThatHalfBlock == null)
                    return false; // no such index
                if (//(blockBelowThatHalfBlock.IsFullBlock //full block below
                    //&& blockBelowThatHalfBlock != World.AirBlock) // there is half block below in right position        
                    //||
                   blockBelowThatHalfBlock != World.AirBlock
                   || world.GetBlock(bottomBlockCoords, Side.Top) != World.AirBlock
                   || world.GetBlock(GetIntegerCoords(), Side.Bottom) != World.AirBlock
                   || sideSnapping == Side.Bottom && world.HasAnyNonAirBlockExcept(bottomBlockCoords, Side.Bottom)
                   )
                    return true;
                else
                    return false;
            }
        }
        protected bool CanBePlaced(World world)
        {

            var blockPlacementCoords = World.GetIntegerCoords(this.block.transform.position);
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

                    // checks for neighbor cells
                    if (this.IsFullBlock)
                    {
                        if (!world.HasAnyNonAirBlock(blockPlacementCoords)//no any blocks here                        
                            && (HasVerticalSupport(world) || HasHorizontalSupport()))
                            return true;
                        else
                            return false;
                    }
                    else // not full block //can be center part or one of 4 edge parts
                    {
                        if (HasVerticalSupport(world) || HasHorizontalSupport())
                            return true;
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
        }

        protected bool HasHorizontalSupport()
        {
            if (maxLengthWithoutSupport > 0)
            {
                var pos = World.GetIntegerCoords(block.transform.position);
                // scan neighbor cells for support
                var supportArea = World.Get.GetMapElementsWithRadius(pos.x, pos.y - 1, pos.z, maxLengthWithoutSupport);
                if (supportArea.Any(x => x.HasAnyNonAirBlock()))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Creates copy of this object ready to put in a world
        /// </summary>    
        protected GameObject InstantiateCopy()
        {
            // Restores original material, instead of green "allowing" material
            SetMaterial(originalMaterial);

            var newBlock = UnityEngine.Object.Instantiate(this.block);
            newBlock.layer = 0; // placed block wouldn't be ignored by raycast           

            if (this.isTrigger)
                newBlock.GetComponent<Collider>().isTrigger = true;
            else
                newBlock.GetComponent<Collider>().isTrigger = false;
            return newBlock;
        }
        protected void SetMaterial(Material newMaterial)
        {
            if (renderer == null)// if objects hasn't renderer look for it in children
            {
                var children = block.GetComponentsInChildren<MeshRenderer>();
                foreach (var item in children)
                {
                    item.material = newMaterial;
                }
            }
            else
                renderer.material = newMaterial;
        }
        public virtual void Show()
        {
            if (block == null) // if it's first call to show instantiate block from prefabs
            {
                block = UnityEngine.Object.Instantiate(prefab);

                block.transform.parent = GManager.Get.PlayersHands.transform;
                renderer = block.GetComponent<MeshRenderer>();
                if (renderer == null) // if objects hasn't renderer look for it in children
                {
                    var children = block.GetComponentsInChildren<MeshRenderer>();
                    originalMaterial = children[0].material;
                }
                else
                    originalMaterial = renderer.material;
            }


            block.SetActive(true);
            EventHandler<EventArgs> handler = Shown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public virtual void Hide()
        {
            block.SetActive(false);
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArgs> handler = Hidden;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public virtual PlacedBlock Place(World world)
        {
            if (this.CanBePlaced(world))
            {
                var newBlockObject = this.InstantiateCopy();
                newBlockObject.transform.parent = world.transform;

                var placedBlock = PlacedBlock.Add(newBlockObject, this, sideSnapping);

                world.Add(this, sideSnapping);

                EventHandler<EventArgs> handler = Placed;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
                return placedBlock;
            }
            return null;
        }

    }
}