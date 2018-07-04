using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IPlaceable
//{
//}
/// <summary>
/// Represents block which can be placed in world and can be hold in hands
/// </summary>
[Serializable]
public class Placeable// : IPlaceable
{
    [SerializeField] private bool isTrigger;
    [SerializeField] private bool requiresSomeFoundation;
    [SerializeField] private bool allowsEdgePlacing;
    //[SerializeField] private bool allowsMultipleObjectsInCell;

    [Tooltip("Should be about same as gameObject thickness")]
    [SerializeField] private float blockThickness;

    [SerializeField] private MeshRenderer renderer;

    [SerializeField] private GameObject gameObject;

    /// <summary> Original material    
    [SerializeField] private Material material;

    /// <summary> which side of map it is closer - north, south, etc 
    /// 0,0 if it's center (default)
    private Vector2Int sideSnapping;

    /// <summary>
    /// Constructor. Instead, you can set values in inspector
    /// </summary>    
    public Placeable(bool allowsEdgePlacing, GameObject prefab, float blockThickness)
    {
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


        //Debug.Log("Int coordinates: " + new Vector3Int(x, z, y));
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
            this.renderer.material = GManager.Get.buildingAlowedMaterial; // originalMat;
        }
        else
        {
            this.renderer.material = GManager.Get.buildingDeniedMaterial;
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

                // Debug.Log("Looking at (x,z,y)" + hit.point.x + " " + hit.point.z + " " + hit.point.y);

                blockPlacingPosition = GetIntegerCoords(hit.point);

                // allow block to sticks to 1 of 4 side of a cell
                if (this.allowsEdgePlacing)
                {
                    sideSnapping = GetClosestSide(lookingPosition, blockPlacingPosition);

                    blockPlacingPosition.x += sideSnapping.x * (0.5f - this.blockThickness / 2f);
                    blockPlacingPosition.z += sideSnapping.y * (0.5f - this.blockThickness / 2f);
                    if (sideSnapping.y == 0) // rotate block if it's closer to y side
                        this.gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    // this.GameObject.transform.Rotate(0f, 0f, 0f);
                    else
                        this.gameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                    // this.GameObject.transform.Rotate(0f, 90f, 90f);
                }

                //Debug.Log("Looking at (x,z,y)" + coordinates.x + " " + coordinates.z + " " + coordinates.y);
                this.gameObject.transform.position = blockPlacingPosition;
                Debug.Log("Looking at (x,z,y)"
                    + lookingPosition.x + " " + lookingPosition.z + " " + lookingPosition.y
                    + " side is " + sideSnapping);
            }

            //if (EventSystem.current.IsPointerOverGameObject())
            //    return null;// -3; //hovering over UI
            // updates holding block color 
            this.UpdateMaterial();
        }
    }
    bool CanBePlaced(World world)
    {
        var coords = GetIntegerCoords(this.gameObject.transform.position);
        var block = world.GetBlock(coords.x, coords.z, coords.y, sideSnapping);
        if (block == null)
            return false; // wrong index
        else
        {
            if (block == World.AirBlock) // is empty space
            {
                if (this.requiresSomeFoundation) // check if underlying cell exists and not empty
                {
                    var coordsToCheck = coords;
                    coordsToCheck.y -= 1;                    
                    if (world.HasAnyNonAirBlock(coordsToCheck.x, coordsToCheck.z, coordsToCheck.y))
                        return true;
                    else
                        return false;
                }
                else
                    return true;
            }
            //else if (this.allowsMultipleObjectsInCell)
            //    return true;
            else
                return false;
        }
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
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void PlaceBlock(World world)
    {
        if (this != null && this.CanBePlaced(world))
        {
            var coords = Placeable.GetIntegerCoords(this.gameObject.transform.position);
            //if (world.IsCellExists(coords.x, coords.z, coords.y)) // already checked in CanBePlaced()
            {
                var newBlock = this.Instantiate();
                newBlock.transform.parent = world.transform;
                Debug.Log("Placed block in (x,z,y)" + coords.x + " " + coords.z + " " + coords.y);
                world.Add(coords.x, coords.z, coords.y, this, sideSnapping);
            }
        }
    }
}
