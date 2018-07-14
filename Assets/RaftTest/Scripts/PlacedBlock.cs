using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace RaftTest
{
    /// <summary>
    /// Represents block placed in a world
    /// Placeable - type of that block
    /// </summary>
    public class Block : MonoBehaviour, IPlaceable
    {
        [SerializeField] public BlockType Type { get; protected set; }
        [SerializeField] public BlockType.Side SideSnapping { get; protected set; }

        /// <summary> which side of map it is closer - north, south, west, east. default is center
        protected BlockType.Side sideSnapping;
        protected MeshRenderer renderer;
        /// <summary> Original material    
        protected Material originalMaterial;

        public bool IsFullBlock
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool OnlyCenterPlacing
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsTrigger
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler<EventArgs> Hidden;
        public event EventHandler<EventArgs> Shown;

        public static Block Instantiate(BlockType placeable, BlockType.Side side, World world)
        {     
            
            var newBlockObject = UnityEngine.Object.Instantiate(placeable.prefab);

            newBlockObject.layer = 0; // placed block wouldn't be ignored by raycast           

            if (placeable.IsTrigger)
                newBlockObject.GetComponent<Collider>().isTrigger = true;
            else
                newBlockObject.GetComponent<Collider>().isTrigger = false;           


            newBlockObject.transform.parent = world.transform;          

            var placed = newBlockObject.AddComponent<Block>();
            placed.Placeable = placeable;
            placed.SideSnapping = side;
            return placed;
        }

        public Vector3Int GetIntegerCoords()
        {
            Vector3 adjustedCoords = World.AdjustCoords(transform.position);

            int x = Mathf.FloorToInt(adjustedCoords.x);
            int y = Mathf.FloorToInt(adjustedCoords.y);
            int z = Mathf.FloorToInt(adjustedCoords.z);


            return new Vector3Int(x, y, z);
        }

        public virtual void Hide()
        {
         gameObject. SetActive(false);
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<EventArgs> handler = Hidden;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public virtual Block Place(World world)
        {
            if (CanBePlaced(world))
            {
                // Restores original material, instead of green "allowing" material
                SetMaterial(originalMaterial);
                //var placedBlock = PlacedBlock.Instantiate(this, sideSnapping, world);

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

        /// <summary>
        /// Used for manual positioning. Alternatively block can be self positioned in UpdateBlock()
        /// </summary>        
        public void SetPosition(Vector3 position, BlockType.Side side)
        {
            sideSnapping = side;
            position = AdjustSnappingXZ(position);
            position = AdjustSnappingY(position);
            AdjustSnappingRotation();
            transform.position = position;
        }

        public virtual void Show()
        {
            if (block == null) // if it's first call to show instantiate block from prefabs
            {
                block = UnityEngine.Object.Instantiate(Type.prefab);

                transform.parent = GManager.Get.PlayersHands.transform;
                renderer = GetComponent<MeshRenderer>();
                if (renderer == null) // if objects hasn't renderer look for it in children
                {
                    var children = GetComponentsInChildren<MeshRenderer>();
                    originalMaterial = children[0].material;
                }
                else
                    originalMaterial = renderer.material;
            }


            gameObject.SetActive(true);
            EventHandler<EventArgs> handler = Shown;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
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
                if (Type.allowsXZSnapping && Type.allowsYSnapping)
                {
                    sideSnapping = World.GetClosestSideXYZ(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingXZ(blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingY(blockPlacingPosition);
                }
                else if (Type.allowsXZSnapping)
                {
                    sideSnapping = World.GetClosestSideXZ(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingXZ(blockPlacingPosition);
                }
                else if (Type.allowsYSnapping)
                {
                    sideSnapping = World.GetClosestSideY(lookingPosition, blockPlacingPosition);
                    blockPlacingPosition = AdjustSnappingY(blockPlacingPosition);
                }


                AdjustSnappingRotation();


                transform.position = blockPlacingPosition;
                Debug.Log("Looking at (x,y,z)" + lookingPosition + " side is " + sideSnapping);

                // updates holding block color 
                this.UpdateMaterial();
            }
        }
        protected bool CanBePlaced(World world)
        {

            var blockPlacementCoords = World.GetIntegerCoords(transform.position);
            var placeToBuild = world.GetBlock(blockPlacementCoords, sideSnapping);

            if (placeToBuild == null)
                return false; // wrong index
            else
            {
                if (placeToBuild == World.AirBlock && !placeToBuild.IsFullBlock) // is empty space                                                                             
                {
                    // here go all kinds of foundation checks
                    if (!Type.requiresSomeFoundation)
                        return true;

                    if (Type.canBePlacedAtZeroLevelWithoutFoundation && blockPlacementCoords.y == 0)
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
        protected void SetMaterial(Material newMaterial)
        {
            if (renderer == null)// if objects hasn't renderer look for it in children
            {
                var children = GetComponentsInChildren<MeshRenderer>();
                foreach (var item in children)
                {
                    item.material = newMaterial;
                }
            }
            else
                renderer.material = newMaterial;
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
        protected Vector3 AdjustSnappingY(Vector3 blockPlacingPosition)
        {
            if (sideSnapping == BlockType.Side.Top)
                blockPlacingPosition.y += (0.5f - Type.blockThickness / 2f);
            else if (sideSnapping == BlockType.Side.Bottom)
                blockPlacingPosition.y -= (0.5f - Type.blockThickness / 2f);
            return blockPlacingPosition;
        }
        protected Vector3 AdjustSnappingXZ(Vector3 blockPlacingPosition)
        {
            if (sideSnapping == BlockType.Side.East)
                blockPlacingPosition.x += (0.5f - Type.blockThickness / 2f);
            else if (sideSnapping == BlockType.Side.West)
                blockPlacingPosition.x -= (0.5f - Type.blockThickness / 2f);
            else if (sideSnapping == BlockType.Side.North)
                blockPlacingPosition.z += (0.5f - Type.blockThickness / 2f);
            else if (sideSnapping == BlockType. Side.South)
                blockPlacingPosition.z -= (0.5f - Type.blockThickness / 2f);
            return blockPlacingPosition;
        }
        protected void AdjustSnappingRotation()
        {
            if (sideSnapping == BlockType.Side.North || sideSnapping == BlockType.Side.South) // rotate block at Y axis depending on what side is closer
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);

            if (sideSnapping == BlockType.Side.East || sideSnapping == BlockType.Side.West) // rotate block at Y axis depending on what side is closer
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            if (sideSnapping == BlockType.Side.Top || sideSnapping == BlockType.Side.Bottom) // rotate block at Z axis depending on what side is closer
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        protected bool HasVerticalSupport(World world)
        {
            var bottomBlockCoords = World.GetIntegerCoords(transform.position) + Vector3Int.down;
            if (IsFullBlock)
            {
                if (world.HasAnyNonAirBlockExcept(bottomBlockCoords, BlockType.Side.Bottom))
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
                   || world.GetBlock(bottomBlockCoords, BlockType.Side.Top) != World.AirBlock
                   || world.GetBlock(GetIntegerCoords(), BlockType.Side.Bottom) != World.AirBlock
                   || sideSnapping == BlockType.Side.Bottom && world.HasAnyNonAirBlockExcept(bottomBlockCoords, BlockType.Side.Bottom)
                   )
                    return true;
                else
                    return false;
            }
        }


        protected bool HasHorizontalSupport()
        {
            if (Type.maxLengthWithoutSupport > 0)
            {
                var pos = World.GetIntegerCoords(transform.position);
                // scan neighbor cells for support
                var supportArea = World.Get.GetMapElementsWithRadius(pos.x, pos.y - 1, pos.z, Type.maxLengthWithoutSupport);
                if (supportArea.Any(x => x.HasAnyNonAirBlock()))
                    return true;
            }
            return false;
        }
    }
}