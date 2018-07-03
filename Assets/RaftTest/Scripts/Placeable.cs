using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IPlaceable
//{
//}
/// <summary>
/// Represents block which can be placed in world
/// </summary>
[Serializable]
public class Placeable// : IPlaceable
{
    [SerializeField] private bool allowsMultipleObjectsInCell;// MultipleObjectsInCell isn't fully implemented    

    [Tooltip("Should be about same as gameObject thickness")]
    [SerializeField] private float blockThickness;

    [SerializeField] private MeshRenderer renderer;

    [SerializeField] private GameObject gameObject;
    public GameObject GameObject { get { return gameObject; } }// todo hide it from public?

    [SerializeField] private Material originalMat;

    /// <summary>
    /// Constructor. Instead, you can set values in inspector
    /// </summary>    
    public Placeable(bool allowsMultipleObjectsInCell, GameObject prefab, float blockThickness)
    {
        this.gameObject = prefab;
        this.allowsMultipleObjectsInCell = allowsMultipleObjectsInCell;
        if (GameObject != null)
            renderer = GameObject.GetComponent<MeshRenderer>();
        this.blockThickness = blockThickness;
    }

    public static Vector3Int GetIntegerCoords(Vector3 position)
    {
        Vector3 AdjustedCoords = World.AdjustCoords(position);

        int x = Mathf.FloorToInt(AdjustedCoords.x);
        int y = Mathf.FloorToInt(AdjustedCoords.y);
        int z = Mathf.FloorToInt(AdjustedCoords.z);

        
        //Debug.Log("Int coordinates: " + new Vector3Int(x, z, y));
        return new Vector3Int(x, y, z);

    }
    /// <summary>
    /// returns which side of map is closer to point - north, south, etc
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

    public void UpdateColor()
    {
        if (CanBePlaced(World.Get))
        {
            if (Input.GetMouseButtonUp(0))
            {
                this.renderer.material = originalMat;
                World.Get.PlaceBlock(this);
            }
            this.renderer.material = GManager.Get.buildingAlowedMaterial; // originalMat;
        }
        else
        {
            this.renderer.material = GManager.Get.buildingDeniedMaterial;
        }
    }

    /// <summary>
    /// updates block held by player - rotates, changes color if building not allowed
    /// </summary>
    public void UpdateHoldingBlock()
    {
        if (this != null)
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Vector3 lookingPosition = World.AdjustCoords(hit.point);
                Vector3 blockPlacingPosition;
                Debug.Log("Looking at (x,z,y)" + lookingPosition.x + " " + lookingPosition.z + " " + lookingPosition.y);
                // Debug.Log("Looking at (x,z,y)" + hit.point.x + " " + hit.point.z + " " + hit.point.y);

                blockPlacingPosition = GetIntegerCoords(hit.point);

                // allow block to sticks to 1 of 4 side of a cell
                if (this.allowsMultipleObjectsInCell)
                {
                    Vector2Int lookingAtSide = Placeable.GetClosestSide(lookingPosition, blockPlacingPosition);

                    blockPlacingPosition.x += lookingAtSide.x * (0.5f - this.blockThickness / 2f);
                    blockPlacingPosition.z += lookingAtSide.y * (0.5f - this.blockThickness / 2f);
                    if (lookingAtSide.y == 0) // rotate block if it's closer to y side
                        this.GameObject.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    else
                        this.GameObject.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                //Debug.Log("Looking at (x,z,y)" + coordinates.x + " " + coordinates.z + " " + coordinates.y);
                this.GameObject.transform.position = blockPlacingPosition;
            }

            //if (EventSystem.current.IsPointerOverGameObject())
            //    return null;// -3; //hovering over UI
            // updates holding block color 
            this.UpdateColor();
        }
    }
    public bool CanBePlaced(World world)
    {
        var coords = GetIntegerCoords(this.gameObject.transform.position);
        var cell = world.GetCell(coords.x, coords.z, coords.y);
        if (cell == null)
            return false; // wrong index
        else
        {
            if (cell == world.AirBlock)
            {
                // todo put it in Placeable?
                if (this.allowsMultipleObjectsInCell) // is wall
                {
                    // check if underlying cell exists and not empty
                    var coordsToCheck = coords;
                    coordsToCheck.y -= 1;
                    var uderlyingCell = world.GetCell(coordsToCheck.x, coordsToCheck.z, coordsToCheck.y);
                    if (uderlyingCell == null || uderlyingCell == world.AirBlock)
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
}
