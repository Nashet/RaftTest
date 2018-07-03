using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    //[SerializeField] public Placeable block1, block2;
    
    //[SerializeField] public Material originalMat1, originalMat2;
    [SerializeField] public Material buildingDenialMaterial;
    [SerializeField] public Placeable[] allBlocks;
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
        //block1 = new Placeable(false, null, blockThickness: 1f);
        //block2 = new Placeable(true, null, blockThickness: 0.2f);
    }
}
