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
    private bool edgeOnlyPlacable;
    public GameObject gameObject;   
    public Placeable(bool edgeOnlyPlacable, GameObject prefab)
    {
        this.gameObject = prefab;
        this.edgeOnlyPlacable = edgeOnlyPlacable;
    }
}
