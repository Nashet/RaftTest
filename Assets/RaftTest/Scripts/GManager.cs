using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    [SerializeField] public Placeable block1, block2;
    [SerializeField] private GameObject prefab1, prefab2;
    [SerializeField] public Material originalMat1, originalMat2;

    static GManager thisObject;
    // allows static access
    public static GManager Get
    {
        get { return thisObject; }
    }
    // Use this for initialization
    void Awake()
    {
        thisObject = this;
        // could be read from a config file
        block1 = new Placeable(false, prefab1, blockThickness: 1f);
        block2 = new Placeable(true, prefab2, blockThickness: 0.2f);
    }
}
