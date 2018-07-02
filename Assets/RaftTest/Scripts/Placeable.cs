using System.Collections;
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
    public bool allowsMultipleObjectsInCell;
    public GameObject gameObject;

    /// <summary>
    /// Constructor
    /// </summary>    
    public Placeable(bool allowsMultipleObjectsInCell, GameObject prefab)
    {
        this.gameObject = prefab;
        this.allowsMultipleObjectsInCell = allowsMultipleObjectsInCell;
        if (gameObject != null)
            renderer = gameObject.GetComponent<MeshRenderer>();
    }

    public Vector3Int GetCoordinats()
    {
        int x = (int)gameObject.transform.position.x * World.Get.blockSize;
        int y = (int)gameObject.transform.position.z * World.Get.blockSize;
        int z = (int)gameObject.transform.position.y * World.Get.blockSize;
        return new Vector3Int(x, z, y);
    }


    public MeshRenderer renderer { get; private set; }
}
