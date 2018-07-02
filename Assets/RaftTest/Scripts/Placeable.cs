using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public interface IPlaceable
//{
//}
public class Placeable// : IPlaceable
{
    private bool edgeOnlyPlacable;
    public GameObject gameObject;
    public Placeable(bool edgeOnlyPlacable, GameObject prefab)
    {
        gameObject = prefab;
        this.edgeOnlyPlacable = edgeOnlyPlacable;
    }
}
