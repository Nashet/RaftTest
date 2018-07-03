﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IPlaceable
//{
//}
/// <summary>
/// Represents block which can be placed in world
/// </summary>
public class Placeable// : IPlaceable
{
    public bool AllowsMultipleObjectsInCell { get; private set; }
    public GameObject gameObject { get; private set; }
    public float BlockThickness { get; private set; }
    public MeshRenderer renderer { get; private set; }
    //private float blockThickness;
    /// <summary>
    /// Constructor
    /// </summary>    
    public Placeable(bool allowsMultipleObjectsInCell, GameObject prefab, float blockThickness)
    {
        this.gameObject = prefab;
        this.AllowsMultipleObjectsInCell = allowsMultipleObjectsInCell;
        if (gameObject != null)
            renderer = gameObject.GetComponent<MeshRenderer>();
        this.BlockThickness = blockThickness;
    }

    public Vector3Int GetIntCoords()
    {
        Vector3 AdjustedCoords = World.AdjustCoords(gameObject.transform.position);
        
        int x = Mathf.FloorToInt(AdjustedCoords.x);
        int y = Mathf.FloorToInt(AdjustedCoords.z);
        int z = Mathf.FloorToInt(AdjustedCoords.y);
        //Debug.Log("Int coordinates: " + new Vector3Int(x, z, y));
        return new Vector3Int(x, z, y);

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
            return new Vector2Int(0, 1);
    }    

}
